using Mela.AuthService.Api.Entities;

namespace Mela.AuthService.Api.Responses;

/// <summary>
///     Ответ на запрос авторизации
/// </summary>
/// <param name="Email">Адрес электронной почты</param>
/// <param name="Status">Статус пользователя</param>
/// <param name="Type">Тип пользователя</param>
/// <param name="JwtToken">Токен авторизации</param>
public record SignInResponse(string Email, UserStatus Status, UserType Type, string JwtToken);