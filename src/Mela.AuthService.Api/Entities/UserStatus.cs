namespace Mela.AuthService.Api.Entities;

public enum UserStatus
{
    /// <summary>
    ///     Статус неизвестен
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    ///     Не подтвержденный пользователь
    /// </summary>
    NotConfirmed = 1,
 
    /// <summary>
    ///     Подтвержденный пользователь
    /// </summary>
    Confirmed = 2,
    
    /// <summary>
    ///     Администратор
    /// </summary>
    Admin = 3,
}