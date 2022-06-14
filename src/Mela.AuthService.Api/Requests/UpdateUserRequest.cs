namespace Mela.AuthService.Api.Requests;

/// <summary>
///     Запрос на обновление данных пользователя
/// </summary>
/// <param name="Id">Идентификатор пользователя</param>
/// <param name="Email">Обновленный адрес электронной почты</param>
/// <param name="Password">Обновленный пароль</param>
public record UpdateUserRequest(long Id, string Email, string Password);