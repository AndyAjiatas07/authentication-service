using AuthService.Domain.Entities;
namespace AuthService.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(string id);
    Task<Role?> GetByNameAsync(string name);

    Task<int> CountUsersByRoleIdAsync(string roleId);
    Task<int> CountUsersByRoleNameAsync(string roleName);

    Task<IReadOnlyList<User>> GetUsersByRoleIdAsync(string roleId);
    Task<IReadOnlyList<User>> GetUsersByRoleNameAsync(string roleName);

    Task<IReadOnlyList<string>> GetUserRoleNamesAsync(string userId);
}