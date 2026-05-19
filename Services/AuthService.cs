using CNPMFastFood.Data;
using CNPMFastFood.Models;

namespace CNPMFastFood.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // ================= REGISTER =================

        public void Register(User user)
        {
            // Không cho user tự tạo role admin
            // Dù form có gửi Role = "admin" thì vẫn ép thành "user"
            user.Role = "user";

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // ================= LOGIN =================

        public User? Login(string account, string password)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Username == account ||
                    x.Email == account);

            if (user == null)
                return null;

            if (string.IsNullOrEmpty(user.Password))
                return null;

            try
            {
                bool check = BCrypt.Net.BCrypt.Verify(
                    password,
                    user.Password);

                return check ? user : null;
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        // ================= CHECK USERNAME =================

        public bool UsernameExists(string username)
        {
            return _context.Users
                .Any(x => x.Username == username);
        }

        // ================= CHECK EMAIL =================

        public bool EmailExists(string email)
        {
            return _context.Users
                .Any(x => x.Email == email);
        }
    }
}