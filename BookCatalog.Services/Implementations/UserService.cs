using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using BookCatalog.Services.Validators;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Services.Implementations;

public class UserService : IUserService
{
    private readonly DataDbContext _dataDbContext;
    private readonly IPresetDataConfiguration _presetDataConfiguration;
    private readonly IPasswordHasher<object> _passwordHasher;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserRoleService _userRoleService;

    public UserService(
        DataDbContext dataDbContext, 
        IPresetDataConfiguration presetDataConfiguration, 
        IPasswordHasher<object> passwordHasher, 
        IRefreshTokenService refreshTokenService, 
        IUserRoleService userRoleService)
    {
        _dataDbContext = dataDbContext;
        _presetDataConfiguration = presetDataConfiguration;
        _passwordHasher = passwordHasher;
        _refreshTokenService = refreshTokenService;
        _userRoleService = userRoleService;
    }

    public UserDto Create(string name, string password, DateTime utcNow)
    {
        var user = new UserDto
        {
            Name = name,
            PasswordHash = HashPassword(password),
            Role = _presetDataConfiguration.DefaultUserRole,
            RefreshToken = _refreshTokenService.Create(utcNow)
        };

        return user;
    }

    public bool Exists(string name) => _dataDbContext.Users.Any(x => x.Name == name);
    public string HashPassword(string password) => _passwordHasher.HashPassword(null, password);

    public bool TryGetModel(string name, out UserDto user)
    {
        user = GetUserDto(x => x.Name == name);

        return user != null;
    }

    public bool TryGetModel(string name, string password, out UserDto user)
    {
        user = GetUserDto(x => x.Name == name);

        if (user == null)
        {
            return false;
        }

        bool passwordMatched = 
            _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password) ==
            PasswordVerificationResult.Success;

        if (passwordMatched)
        {
            return true;
        }
        
        user = default;
        return false;
    }

    public void Save(UserDto userDto)
    {
        if (!new UserDtoValidator().Validate(userDto, out string error))
        {
            throw new ValidationException(error);
        }

        UserRecord userRecord = userDto.Id == default ? CreateRecord(userDto) : UpdateRecord(userDto);
        
        _dataDbContext.SaveChanges();
        userDto.Id = userRecord.Id;
    }

    private UserRecord CreateRecord(UserDto userDto)
    {
        var userRecord = new UserRecord
        {
            Name = userDto.Name,
            PasswordHash = userDto.PasswordHash
        };

        if (!string.IsNullOrEmpty(userDto.Role))
        {
            userRecord.UserRole = _userRoleService.CreateRecord(userDto.Role);
        }

        if (userDto.RefreshToken != null)
        {
            userRecord.UserRefreshToken = _refreshTokenService.CreateRecord(userDto.RefreshToken);
        }
        
        _dataDbContext.Users.Add(userRecord);

        return userRecord;
    }

    private UserRecord UpdateRecord(UserDto user)
    {
        UserRecord userRecord = GetUserRecord(x => x.Id == user.Id);

        if (userRecord == null)
        {
            throw new KeyNotFoundException($"User with id: {user.Id} not found.");
        }
        
        userRecord.Name = user.Name;
        userRecord.PasswordHash = user.PasswordHash;

        if (userRecord.UserRole == null)
        {
            userRecord.UserRole = _userRoleService.CreateRecord(user.Role);
        }
        else
        {
            _userRoleService.UpdateRecord(userRecord.UserRole, user.Role);
        }

        if (userRecord.UserRefreshToken == null)
        {
            userRecord.UserRefreshToken = _refreshTokenService.CreateRecord(user.RefreshToken);
        }
        else
        {
            _refreshTokenService.UpdateRecord(userRecord.UserRefreshToken, user.RefreshToken);
        }

        return userRecord;
    }

    private UserDto GetUserDto(Expression<Func<UserRecord, bool>> predicate)
    {
        UserRecord userRecord = GetUserRecord(predicate);

        if (userRecord == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = userRecord.Id,
            Name = userRecord.Name,
            PasswordHash = userRecord.PasswordHash,
            Role = userRecord.UserRole?.Role?.Name,
            RefreshToken = 
                userRecord.UserRefreshToken != null ? _refreshTokenService.Create(userRecord.UserRefreshToken) : null
        };
    }

    private UserRecord GetUserRecord(Expression<Func<UserRecord, bool>> predicate)
    {
        return _dataDbContext.Users
            .Where(predicate)
            .Include(x => x.UserRole)
            .Include(x => x.UserRole.Role)
            .Include(x => x.UserRefreshToken)
            .FirstOrDefault();
    }
}