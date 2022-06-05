using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Mela.AuthService.Api.Options;

public class AuthOptions
{
    /// <summary>
    ///     Издатель токена
    /// </summary>
    public const string Issuer = "MelaAuthService";
    
    /// <summary>
    ///     Потребитель токена
    /// </summary>
    public const string Audience = "MelaService";
    
    /// <summary>
    ///     Ключ для шифрования
    /// </summary>
    private const string Key = "18dc959e-1378-43b0-8152-a1f3611c92a2";
    
    /// <summary>
    ///     Передача ключа для шифрования
    /// </summary>
    /// <returns></returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}