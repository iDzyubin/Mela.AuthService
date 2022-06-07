namespace Mela.AuthService.Api.Entities;

public class User : Entity
{
    /// <summary>
    ///     Email пользователя
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    ///     Пароль пользователя
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    ///     Статус пользователя
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Unknown;

    /// <summary>
    ///     Тип пользователя
    /// </summary>
    public UserType Type { get; set; }
    
    /// <summary>
    ///     Код подтверждения
    /// </summary>
    public string ConfirmCode { get; set; } = null!;
}