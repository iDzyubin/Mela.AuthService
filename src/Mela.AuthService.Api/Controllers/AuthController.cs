using Microsoft.AspNetCore.Mvc;

namespace Mela.AuthService.Api.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}