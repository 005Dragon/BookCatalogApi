using BookCatalog.Services.Contracts;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class UserRoleService : IUserRoleService
{
    private readonly DataDbContext _dataDbContext;

    public UserRoleService(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public UserRoleRecord CreateRecord(string name)
    {
        RoleRecord roleRecord = GetRoleRecord(name);
        
        var userRoleRecord = new UserRoleRecord { RoleId = roleRecord.Id, };
        
        return userRoleRecord;
    }

    public void UpdateRecord(UserRoleRecord userRoleRecord, string name)
    {
        userRoleRecord.RoleId = GetRoleRecord(name).Id;
    }

    private RoleRecord GetRoleRecord(string name)
    {
        RoleRecord roleRecord = _dataDbContext.Roles.FirstOrDefault(x => x.Name == name);
        
        if (roleRecord == null)
        {
            throw new InvalidOperationException("Invalid role.");
        }

        return roleRecord;
    }
}