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
            user.Role = "user";

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // ================= REGISTER EXTERNAL USER =================
        // Dùng cho Google/Facebook login
        public User RegisterExternalUser(string email, string? name)
        {
            var username = email.Split('@')[0];

            if (UsernameExists(username))
            {
                username = username + "_" + Guid.NewGuid().ToString("N")[..6];
            }

            var password = Guid.NewGuid().ToString("N");

            var user = new User
            {
                Username = username,
                Email = email,
                Password = password,
                ConfirmPassword = password,
                Role = "user",
                IsBlocked = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        // ================= LOGIN =================
        public User? Login(string account, string password)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Username == account ||
                    x.Email == account);

            if (user == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            if (password == user.Password)
            {
                return user;
            }

            return null;
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

        // ================= GET USER BY EMAIL =================
        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(x => x.Email == email);
        }

        // ================= RESET PASSWORD =================
        public bool ResetPassword(string email, string newPassword)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;

            _context.SaveChanges();

            return true;
        }
        
        public User? GetById(int id)
        {
            return _context.Users
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}