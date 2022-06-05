namespace Mela.AuthService.Api.Requests;

public class ConfirmRequest
{
    /// <summary>
    ///     Адрес электронной почты
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Код подтверждения
    /// </summary>
    public string ConfirmCode { get; set; }
}