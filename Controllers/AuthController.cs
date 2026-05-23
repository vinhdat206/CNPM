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
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth");

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var authenticateResult =
                await HttpContext.AuthenticateAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                TempData["Error"] = "Đăng nhập Google/Facebook thất bại.";
                return RedirectToAction("Login");
            }

            var email = authenticateResult.Principal?
                .FindFirstValue(ClaimTypes.Email);

            var name = authenticateResult.Principal?
                .FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Không lấy được email từ tài khoản.";
                return RedirectToAction("Login");
            }

            var user = _authService.GetByEmail(email);

            if (user == null)
            {
                user = _authService.RegisterExternalUser(email, name);
            }

            if (user.IsBlocked)
            {
                TempData["Error"] = "Tài khoản của bạn đã bị khóa.";
                return RedirectToAction("Login");
            }

            await SignInUser(user, false);

            if (user.Role == "admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
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

            if (user.IsBlocked)
            {
                ViewBag.Error = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";
                return View();
            }

            await SignInUser(user, RememberMe);

            if (user.Role == "admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
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

            if (!PasswordHelper.IsMatch(model.Password, model.ConfirmPassword))
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

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _authService.GetByEmail(model.Email);

            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống";
                return View(model);
            }

            return RedirectToAction("ResetPassword", new { email = model.Email });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!PasswordHelper.IsStrongPassword(model.NewPassword))
            {
                ViewBag.Error =
                    "Mật khẩu phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";
                return View(model);
            }

            bool success = _authService.ResetPassword(
                model.Email,
                model.NewPassword);

            if (!success)
            {
                ViewBag.Error = "Email không tồn tại";
                return View(model);
            }

            TempData["Success"] = "Đổi mật khẩu thành công";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(
            string OldPassword,
            string NewPassword,
            string ConfirmPassword)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("Profile");
            }

            var user = _authService.GetById(int.Parse(userId));

            if (user == null)
            {
                TempData["Error"] = "Tài khoản không tồn tại.";
                return RedirectToAction("Profile");
            }

            if (user.Password != OldPassword)
            {
                TempData["Error"] = "Mật khẩu cũ không đúng.";
                return RedirectToAction("Profile");
            }

            if (!PasswordHelper.IsStrongPassword(NewPassword))
            {
                TempData["Error"] =
                    "Mật khẩu mới phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";
                return RedirectToAction("Profile");
            }

            if (NewPassword != ConfirmPassword)
            {
                TempData["Error"] = "Xác nhận mật khẩu mới không khớp.";
                return RedirectToAction("Profile");
            }

            user.Password = NewPassword;
            user.ConfirmPassword = NewPassword;

            _authService.Update(user);

            TempData["Success"] = "Đổi mật khẩu thành công.";

            return RedirectToAction("Profile");
        }

        private async Task SignInUser(User user, bool rememberMe)
        {
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
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);
        }
    }
}