using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mela.AuthService.Api.Contexts;
using Mela.AuthService.Api.Entities;
using Mela.AuthService.Api.Helpers;
using Mela.AuthService.Api.Options;
using Mela.AuthService.Api.Requests;
using Mela.AuthService.Api.Responses;
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
    private readonly UserContext _context;
    private readonly IMailService _mailService;

    public AuthController(UserContext context, IMailService mailService)
    {
        _context = context;
        _mailService = mailService;
    }

    /// <summary>
    ///     Авторизация пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null) return NotFound();

        var isPasswordValid = PasswordHashHelper.ValidateHash(password: request.Password, hashedPassword: user.Password);
        if (!isPasswordValid) return BadRequest();
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: new []{ new Claim(ClaimTypes.Email, request.Email) },
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        return Ok(new SignInResponse(request.Email, user.Status, user.Type, new JwtSecurityTokenHandler().WriteToken(jwt)));
    }

    /// <summary>
    ///     Регистрация пользователя
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user != null) return BadRequest("Пользователь с таким Email уже существует");

        var confirmCode = await _mailService.SendConfirmCodeAsync(request.Email);
        if (string.IsNullOrWhiteSpace(confirmCode))
            return BadRequest("Не удалось отправить код подтверждения");

        var result = await _context.AddAsync(new User
        {
            Email = request.Email,
            Status = UserStatus.NotConfirmed,
            Type = Enum.TryParse(typeof(UserType), request.Type, out var userType) && userType != null
                ? (UserType) userType 
                : UserType.Unknown,
            ConfirmCode = PasswordHashHelper.ComputeHash(confirmCode),
            Password = PasswordHashHelper.ComputeHash(request.Password),
        });
        await _context.SaveChangesAsync();
        
        return Ok(new SignUpResponse(result.Entity.Id, request.Email, "Пользователь зарегистрирован. Требуется подтверждение учетной записи"));
    }

    /// <summary>
    ///     Подтверждение пользователя с помощью кода из сообщения
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Confirm([FromForm] ConfirmRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Email == request.Email);
        if (user == null) return BadRequest();

        var isValidCode = PasswordHashHelper.ValidateHash(request.ConfirmCode, user.ConfirmCode);
        if (!isValidCode) return BadRequest();

        user.Status = UserStatus.Confirmed;
        await _context.SaveChangesAsync();

        return Ok(new ConfirmResponse(request.Email, "Учетная запись пользователя успешно подтверждена"));
    }
}