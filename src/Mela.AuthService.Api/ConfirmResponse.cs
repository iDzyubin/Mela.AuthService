namespace Mela.AuthService.Api;

/// <summary>
///     Ответ на запрос подтверждения учетной записи
/// </summary>
/// <param name="Email">Адрес электронной почты</param>
/// <param name="Message">Сообщение</param>
public record ConfirmResponse(string Email, string Message);