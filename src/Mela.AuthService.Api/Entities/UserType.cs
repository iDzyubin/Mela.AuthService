namespace Mela.AuthService.Api.Entities;

/// <summary>
///     Тип пользователя
/// </summary>
public enum UserType
{
    /// <summary>
    ///     Тип неизвестен
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    ///     Пациент
    /// </summary>
    Patient = 1,
    
    /// <summary>
    ///     Доктор
    /// </summary>
    Doctor = 2,
    
    /// <summary>
    ///     Администратор
    /// </summary>
    Admin = 3,
}