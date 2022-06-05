namespace Mela.AuthService.Api.Requests;

public class SignInRequest
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