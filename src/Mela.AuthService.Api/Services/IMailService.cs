namespace Mela.AuthService.Api.Services;

/// <summary>
///     Клиент для сервиса формирования сообщений
/// </summary>
public interface IMailService
{
    /// <summary>
    ///     Отправить код подтверждения на адрес пользователя
    /// </summary>
    /// <param name="email">Адрес пользователя</param>
    /// <returns>Код подтверждения</returns>
    Task<string?> SendConfirmCodeAsync(string email);
}