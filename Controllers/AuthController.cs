using CNPMFastFood.Helpers; // Chứa các hàm hỗ trợ như kiểm tra mật khẩu mạnh
using CNPMFastFood.Models; // Chứa các model User, ForgotPasswordViewModel,...
using CNPMFastFood.Services; // Chứa AuthService xử lý nghiệp vụ
using Microsoft.AspNetCore.Authentication; // Hỗ trợ Authentication
using Microsoft.AspNetCore.Authentication.Cookies; // Hỗ trợ Cookie Authentication
using Microsoft.AspNetCore.Mvc; // Dùng cho MVC Controller
using System.Security.Claims; // Dùng để tạo Claims lưu thông tin user

namespace CNPMFastFood.Controllers
{
    // Controller xử lý các chức năng xác thực người dùng
    public class AuthController : Controller
    {
        // Service xử lý logic tài khoản
        private readonly AuthService _authService;

        // Constructor Dependency Injection
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // ==========================
        // ĐĂNG NHẬP GOOGLE/FACEBOOK
        // ==========================

        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            // URL callback sau khi đăng nhập thành công
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth");

            // Thiết lập thông tin xác thực
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            // Chuyển hướng tới Google/Facebook
            return Challenge(properties, provider);
        }

        // Callback sau khi Google/Facebook xác thực thành công
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var authenticateResult =
                await HttpContext.AuthenticateAsync("External");

            if (!authenticateResult.Succeeded)
            {
                TempData["Error"] =
                    authenticateResult.Failure?.Message
                    ?? "Đăng nhập thất bại.";

                return RedirectToAction("Login");
            }

            var email = authenticateResult.Principal?
                .FindFirstValue(ClaimTypes.Email);

            var name = authenticateResult.Principal?
                .FindFirstValue(ClaimTypes.Name);

            var providerId = authenticateResult.Principal?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(providerId))
            {
                TempData["Error"] = "Không lấy được ID tài khoản Facebook/Google.";
                return RedirectToAction("Login");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "External User";
            }

            if (string.IsNullOrEmpty(email))
            {
                email = $"facebook_{providerId}@facebook.local";
            }

            var user = _authService.GetByEmail(email);

            if (user == null)
            {
                user = _authService.RegisterExternalUser(email, name);
            }

            if (user.IsBlocked)
            {
                TempData["Error"] = "Tài khoản đã bị khóa.";
                return RedirectToAction("Login");
            }

            await SignInUser(user, false);

            await HttpContext.SignOutAsync("External");

            if (user.Role == "admin")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard",
                    new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        // ==========================
        // LOGIN
        // ==========================

        // Hiển thị trang Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(
            string account,
            string password,
            bool RememberMe)
        {
            // Kiểm tra tài khoản và mật khẩu
            var user = _authService.Login(account, password);

            // Nếu sai thông tin
            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            // Kiểm tra tài khoản bị khóa
            if (user.IsBlocked)
            {
                ViewBag.Error = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";
                return View();
            }

            // Đăng nhập
            await SignInUser(user, RememberMe);

            // Nếu là admin
            if (user.Role == "admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            // User thường
            return RedirectToAction("Index", "Home");
        }

        // ==========================
        // REGISTER
        // ==========================

        // Hiển thị form đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public IActionResult Register(User model)
        {
            // Kiểm tra username đã tồn tại chưa
            if (_authService.UsernameExists(model.Username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View();
            }

            // Kiểm tra email đã tồn tại chưa
            if (_authService.EmailExists(model.Email))
            {
                ViewBag.Error = "Email đã tồn tại";
                return View();
            }

            // Kiểm tra mật khẩu mạnh
            if (!PasswordHelper.IsStrongPassword(model.Password))
            {
                ViewBag.Error =
                    "Mật khẩu phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";
                return View();
            }

            // Kiểm tra xác nhận mật khẩu
            if (!PasswordHelper.IsMatch(model.Password, model.ConfirmPassword))
            {
                ViewBag.Error = "Xác nhận mật khẩu không đúng";
                return View();
            }

            // Gán quyền mặc định
            model.Role = "user";

            // Lưu tài khoản
            _authService.Register(model);

            // Chuyển sang trang login
            return RedirectToAction("Login");
        }

        // ==========================
        // LOGOUT
        // ==========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Xóa Cookie Authentication
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }

        // ==========================
        // QUÊN MẬT KHẨU
        // ==========================

        // Hiển thị form nhập email
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Xử lý quên mật khẩu
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Tìm user theo email
            var user = _authService.GetByEmail(model.Email);

            // Nếu email không tồn tại
            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống";
                return View(model);
            }

            // Chuyển sang ResetPassword
            return RedirectToAction("ResetPassword", new { email = model.Email });
        }

        // ==========================
        // RESET PASSWORD
        // ==========================

        // Hiển thị form đổi mật khẩu
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email
            };

            return View(model);
        }

        // Xử lý đổi mật khẩu
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            // Kiểm tra dữ liệu
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra mật khẩu mạnh
            if (!PasswordHelper.IsStrongPassword(model.NewPassword))
            {
                ViewBag.Error =
                    "Mật khẩu phải có chữ hoa, chữ thường, ký tự đặc biệt và tối thiểu 8 ký tự.";
                return View(model);
            }

            // Đổi mật khẩu
            bool success = _authService.ResetPassword(
                model.Email,
                model.NewPassword);

            // Nếu email không tồn tại
            if (!success)
            {
                ViewBag.Error = "Email không tồn tại";
                return View(model);
            }

            TempData["Success"] = "Đổi mật khẩu thành công";

            return RedirectToAction("Login");
        }

        // ==========================
        // PROFILE
        // ==========================

        [HttpGet]
        public IActionResult Profile()
        {
            // Kiểm tra đã đăng nhập chưa
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // ==========================
        // ĐỔI MẬT KHẨU
        // ==========================

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

            if (!BCrypt.Net.BCrypt.Verify(OldPassword, user.Password))
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

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);

            user.Password = hashedPassword;
            user.ConfirmPassword = hashedPassword;

            _authService.Update(user);

            TempData["Success"] = "Đổi mật khẩu thành công.";

            return RedirectToAction("Profile");
        }

        // ==========================
        // HÀM ĐĂNG NHẬP HỆ THỐNG
        // ==========================

        // Hàm tạo Cookie Authentication
        private async Task SignInUser(User user, bool rememberMe)
        {
            // Tạo danh sách Claims
            var claims = new List<Claim>
            {
                // Username
                new Claim(ClaimTypes.Name, user.Username),

                // Email
                new Claim(ClaimTypes.Email, user.Email ?? ""),

                // Role
                new Claim(ClaimTypes.Role, user.Role),

                // UserId
                new Claim("UserId", user.Id.ToString()),

                // App Runtime Id
                new Claim("AppStartId", AppRuntime.AppStartId)
            };

            // Tạo Identity
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Tạo Principal
            var principal = new ClaimsPrincipal(identity);

            // Thiết lập Cookie Authentication
            var authProperties = new AuthenticationProperties
            {
                // Ghi nhớ đăng nhập
                IsPersistent = rememberMe,

                // Thời gian hết hạn
                ExpiresUtc = rememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            // Đăng nhập và tạo Cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);
        }
    }
}