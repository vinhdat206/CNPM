using CNPMFastFood.Helpers;
using CNPMFastFood.Models;
using CNPMFastFood.Services;
using Microsoft.AspNetCore.Mvc;

namespace CNPMFastFood.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(
            string username,
            string password)
        {
            var user =
                _authService.Login(username, password);

            if (user == null)
            {
                ViewBag.Error =
                    "Sai tài khoản hoặc mật khẩu";

                return View();
            }

            HttpContext.Session.SetString(
                "Username",
                user.Username);

            HttpContext.Session.SetString(
                "Role",
                user.Role);

            return RedirectToAction(
                "Index",
                "Home");
        }


        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            // check username tồn tại
            if (_authService.UsernameExists(model.Username))
            {
                ViewBag.Error =
                    "Tên đăng nhập đã tồn tại";

                return View();
            }

            // check password mạnh
            if (!PasswordHelper.IsStrongPassword(
                    model.Password))
            {
                ViewBag.Error =
                    "Mật khẩu phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";

                return View();
            }

            // check confirm password
            if (!PasswordHelper.IsMatch(
                    model.Password,
                    model.ConfirmPassword))
            {
                ViewBag.Error =
                    "Xác nhận mật khẩu không đúng";

                return View();
            }

            // đăng ký
            _authService.Register(model);

            return RedirectToAction("Login");
        }


        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Auth");
        }
    }
}