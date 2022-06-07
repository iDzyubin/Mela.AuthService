namespace Mela.AuthService.Api.Requests;

/// <summary>
///     Запрос на регистрацию
/// </summary>
public class SignUpRequest
{
    /// <summary>
    ///     Адрес электронной почты
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     Тип пользователя
    /// </summary>
    public string Type { get; set; }
}