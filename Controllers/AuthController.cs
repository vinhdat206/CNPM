using CNPMFastFood.Helpers;
using CNPMFastFood.Models;
using CNPMFastFood.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CNPMFastFood.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            string account,
            string password,
            bool RememberMe)
        {
            var user = _authService.Login(account, password);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString()),
                new Claim("AppStartId", AppRuntime.AppStartId)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = RememberMe,

                ExpiresUtc = RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            if (user.Role == "admin")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (_authService.UsernameExists(model.Username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View();
            }

            if (_authService.EmailExists(model.Email))
            {
                ViewBag.Error = "Email đã tồn tại";
                return View();
            }

            if (!PasswordHelper.IsStrongPassword(model.Password))
            {
                ViewBag.Error =
                    "Mật khẩu phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";
                return View();
            }

            if (!PasswordHelper.IsMatch(
                model.Password,
                model.ConfirmPassword))
            {
                ViewBag.Error = "Xác nhận mật khẩu không đúng";
                return View();
            }

            model.Role = "user";

            _authService.Register(model);

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Auth");
        }
    }
}