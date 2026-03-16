using AuthService.Domain;
using AuthService.Domain.Entities; 
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(string id)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> CountUsersByRoleIdAsync(string roleId)
    {
        return await context.UserRoles.CountAsync(ur => ur.RoleId == roleId);
    }

    public async Task<IReadOnlyList<User>> GetUsersByRoleIdAsync(string roleId)
    {
        return await context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.User)
            .ToListAsync();
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await context.Roles
            .Include(r => r.UserRoles)
            .FirstOrDefaultAsync(r => r.Name == roleName);
    }

    // 👇 NUEVO NOMBRE
    public async Task<int> CountUsersByRoleNameAsync(string roleName)
    {
        return await context.UserRoles
            .Where(ur => ur.Role.Name == roleName)
            .CountAsync();
    }

    // 👇 NUEVO NOMBRE
    public async Task<IReadOnlyList<User>> GetUsersByRoleNameAsync(string roleName)
    {
        return await context.UserRoles
            .Where(ur => ur.Role.Name == roleName)
            .Select(ur => ur.User)
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmail)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetUserRoleNamesAsync(string userId)
    {
        return await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync();
    }
}