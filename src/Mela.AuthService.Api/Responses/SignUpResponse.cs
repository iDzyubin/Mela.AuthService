namespace Mela.AuthService.Api.Responses;

/// <summary>
///     Ответ на запрос регистрации пользователя
/// </summary>
/// <param name="Id">Идентификатор пользователя</param>
/// <param name="Email">Адрес электронной почты</param>
/// <param name="Message">Сообщение</param>
public record SignUpResponse(long Id, string Email, string Message);