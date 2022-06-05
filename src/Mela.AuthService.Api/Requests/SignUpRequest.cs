namespace Mela.AuthService.Api.Requests;

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
}