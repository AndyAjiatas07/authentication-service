using AuthService.Domain.Entities;
namespace AuthService.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name);
    Task<IReadOnlyList<User>> GetusersByRoleAsync(string roleId);
    Task<IReadOnlyList<string>> GetUserRoleNameAsync(string userId);
    Task<int> CountUsersInRoleAsync(string roleId);
}
