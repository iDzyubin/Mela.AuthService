using Mela.AuthService.Api.Contexts;
using Mela.AuthService.Api.Helpers;
using Mela.AuthService.Api.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mela.AuthService.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserContext _context;

    public UserController(UserContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Обновить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="request">Данные запроса</param>
    [HttpPut("{userId:long}")]
    public async Task<IActionResult> Update(long userId, UpdateUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == userId);
        if (user == null) return NotFound($"Пользователь с идентификатором '{userId}' не найден");

        if (user.Email != request.Email) 
            user.Email = request.Email;

        if (!PasswordHashHelper.ValidateHash(request.Password, user.Password))
            user.Password = PasswordHashHelper.ComputeHash(request.Password);

        await _context.SaveChangesAsync();
        return Ok($"Данные пользователя с идентификатором '{userId}' были успешно обновлены");
    }

    /// <summary>
    ///     Удалить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpDelete("{userId:long}")]
    public async Task<IActionResult> Delete(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == userId);
        if (user == null) return NotFound($"Пользователь с идентификатором '{userId}' не найден");

        user.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Ok($"Данные пользователя с идентификатором '{userId}' были успешно удалены");
    }

    /// <summary>
    ///     Восстановить информацию о пользователе
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpPut("{userId:long}/restore")]
    public async Task<IActionResult> Restore(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.IsDeleted && x.Id == userId);
        if (user == null) return NotFound($"Пользователь с идентификатором '{userId}' не найден");
        
        user.IsDeleted = false;
        await _context.SaveChangesAsync();

        return Ok($"Данные пользователя с идентификатором '{userId}' были успешно восстановлены");
    }
}