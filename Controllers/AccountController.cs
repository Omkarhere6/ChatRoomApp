using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using ChatRoomApp.Interfaces;

public class AccountController : Controller
{
    private readonly IUserDao _iuserDao;
    public AccountController(IUserDao userDao)
    {
        _iuserDao = userDao;
    }

    [HttpPost]
    public async Task<IActionResult> Register(string userName, string password)
    {
        if (await _iuserDao.UserExistsAsync(userName))
        {
            return Json(new { data = "Username already taken" });
        }

        var registrationResult = await _iuserDao.RegisterUserAsync(userName, password);

        if (registrationResult)
        {
            return RedirectToAction("Login");
        }

        return Json(new { data = "An error occurred while creating your account. Please try again." });
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var principal = _iuserDao.AuthenticateUserAsync(userName, password);
        if (principal != null)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Json(new { data = "True" });
        }
        else
        {
            return Json(new { data = "False" });
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
