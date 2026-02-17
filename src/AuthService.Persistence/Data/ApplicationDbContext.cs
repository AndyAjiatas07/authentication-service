using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
 
namespace AuthService.Persistence.Data;
 
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }
   
    //Representacion de tablas en el modelo
    public DbSet<User> Users {get; set; } 
    public DbSet<UserProfile> UserProfiles {get; set; } 
    public DbSet<Role> Roles {get; set; } 
    public DbSet<UserRole> UserRoles {get; set; } 
    public DbSet<UserEmail> UserEmails {get; set; } 
    public DbSet<UserPasswordReset> UserPasswordResets {get; set; } 
 
    //Convierte camelCase a snake_case
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    foreach (var entity in modelBuilder.Model.GetEntityTypes())
    {
        var tableName = entity.GetTableName();
        if (!string.IsNullOrEmpty(tableName))
        {
            entity.SetTableName(ToSnakeCase(tableName));
        }

        foreach (var property in entity.GetProperties())
        {
            var columnName = property.GetColumnName();
            if (!string.IsNullOrEmpty(columnName))
            {
                property.SetColumnName(ToSnakeCase(columnName));
            }
        }
    }
 
    //Configuracion de relaciones de la entidad User
    modelBuilder.Entity<User>(entity =>
    {
        //Llave primaria
        entity.HasIndex(e => e.Id).IsUnique();
 
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasIndex(e => e.Username).IsUnique();
 
        //Relacion con UserProfile
        entity.HasOne(e => e.UserProfile)
        .WithOne(p => p.User)
        .HasForeignKey<UserProfile>(p => p.UserId)  
        .OnDelete(DeleteBehavior.Cascade);
 
        //Relacion con UserRole
        entity.HasMany(e => e.UserRoles)
        .WithOne(ur => ur.User)
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.Cascade);
 
        //Relacion con UserEmail
        entity.HasOne(e => e.UserEmail)
        .WithOne(ue => ue.User)
        .HasForeignKey<UserEmail>(ue => ue.UserId)
        .OnDelete(DeleteBehavior.Cascade);
 
        //Relacion con UserPasswordReset
        entity.HasOne(e => e.PasswordReset)
        .WithOne(upr => upr.User)
        .HasForeignKey<UserPasswordReset>(upr => upr.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    });
 
    //Configuracion de relaciones de la entidad UserRole
    modelBuilder.Entity<UserRole>(entity =>
    {
        entity.HasKey(e => e.Id);
       
        //El usuario no puede tener el mismo rol mas de una vez
        entity.HasIndex(e => new {e.UserId, e.RoleId}).IsUnique();
 
        //
    });
 
    modelBuilder.Entity<Role>(entity =>
    {
        entity.HasKey(e => e.Id);
       
        //El rol no puede tener el mismo nombre mas de una vez
        entity.HasIndex(e => e.Name).IsUnique();
    });
}
   
    //Funcion para configurar el nombre de la clase a nombre de DB
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
 
        return string.Concat(
        input.Select((x, i) => i > 0 && char.IsUpper(x)
            ? "_" + x.ToString().ToLower()
            : x.ToString().ToLower())
            );
    }
}
