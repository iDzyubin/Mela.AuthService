using Mela.AuthService.Api.Entities;
using Mela.AuthService.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Mela.AuthService.Api.Contexts;

/// <summary>
///     Контекст для работы с пользователями
/// </summary>
public sealed class UserContext : DbContext
{
    /// <summary>
    ///     Пользователи
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <inheritdoc cref="DbContext"/>
    public UserContext(DbContextOptions options) : base(options) => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "admin@mela.ru",
                Password = PasswordHashHelper.ComputeHash("knv#&%9R321"),
                Status = UserStatus.Confirmed,
                Type = UserType.Admin,
                ConfirmCode = PasswordHashHelper.ComputeHash("123345")
            });
    }
}