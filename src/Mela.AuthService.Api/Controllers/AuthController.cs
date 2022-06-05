using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Mela.AuthService.Api.Contexts;
using Mela.AuthService.Api.Entities;
using Mela.AuthService.Api.Options;
using Mela.AuthService.Api.Requests;
using Mela.AuthService.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mela.AuthService.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("/api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    /// <summary>
    ///     Авторизация пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SingIn([FromForm] SignInRequest request, [FromServices] UserContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null) return NotFound();

        var isPasswordValid = PasswordHashHelper.ValidateHash(password: request.Password, hashedPassword: user.Password);
        if (!isPasswordValid) return BadRequest();
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: new []{ new Claim(ClaimTypes.Email, request.Email) },
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    /// <summary>
    ///     Регистрация пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] SignUpRequest request, [FromServices] UserContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user != null) return BadRequest("Пользователь с таким Email уже существует");

        var confirmCode = await SendConfirmCodeAsync(request.Email);
        await context.AddAsync(new User
        {
            Email = request.Email,
            Status = UserStatus.NotConfirmed,
            ConfirmCode = PasswordHashHelper.ComputeHash(confirmCode),
            Password = PasswordHashHelper.ComputeHash(request.Password),
        });
        await context.SaveChangesAsync();
        
        return Ok($"Пользователь с email='{request.Email}' зарегистрирован. Требуется подтверждение учетной записи");
    }

    /// <summary>
    ///     Подтверждение пользователя с помощью кода из сообщения
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Confirm([FromForm] ConfirmRequest request, [FromServices] UserContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Email == request.Email);
        if (user == null) return BadRequest();

        var isValidCode = PasswordHashHelper.ValidateHash(request.ConfirmCode, user.ConfirmCode);
        if (!isValidCode) return BadRequest();

        user.Status = UserStatus.Confirmed;
        await context.SaveChangesAsync();

        return Ok($"Учетная запись пользователя с email='{request.Email}' успешно подтверждена");
    }

    /// <summary>
    ///     Отправить код подтверждения на адрес пользователя
    /// </summary>
    /// <param name="email">Адрес пользователя</param>
    /// <returns>Код подтверждения</returns>
    [NonAction]
    private async Task<string> SendConfirmCodeAsync(string email)
    {
        var confirmCode = new Random().Next(100_000, 999_999).ToString();

        var payload = new
        {
            RecipientEmail = email,
            Subject = "Код подтверждения",
            Content = $"Для подтверждения учетной записи введите код подтверждения: {confirmCode}",
        };
        
        await new HttpClient().PostAsync(
            "http://0.0.0.0:10004/api/mail/send", 
            new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, MediaTypeNames.Application.Json));

        return confirmCode;
    }
}