using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Contracts;

public interface IUserRoleService
{
    internal UserRoleRecord CreateRecord(string name);
    internal void UpdateRecord(UserRoleRecord userRoleRecord, string name);
}