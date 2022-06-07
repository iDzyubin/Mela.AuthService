using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Mela.AuthService.Api.Services;

/// <summary>
///     Реализация <see cref="IMailService"/>
/// </summary>
public class MailService : IMailService
{
    private readonly string _connectionString;

    public MailService(IHostEnvironment environment, IConfiguration configuration)
    {
        _connectionString = environment.IsDevelopment()
            ? configuration.GetSection("ServiceConnections")["MailServiceConnection"]
            : Environment.GetEnvironmentVariable("MailServiceConnection") ?? string.Empty;
    }
    
    /// <inheritdoc/>
    public async Task<string?> SendConfirmCodeAsync(string email)
    {
        var confirmCode = new Random().Next(100_000, 999_999).ToString();

        var payload = JsonSerializer.Serialize(new
        {
            RecipientEmail = email,
            Subject = "Код подтверждения",
            Content = $"Для подтверждения учетной записи введите код подтверждения: {confirmCode}",
        });

        var content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await new HttpClient().PostAsync($"{_connectionString}/send", content);

        if (!response.IsSuccessStatusCode)
            return null;
        
        return confirmCode;
    }
}