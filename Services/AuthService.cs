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

        public void Register(User user)
        {
            user.Role = "user";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.ConfirmPassword = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User RegisterExternalUser(string email, string? name)
        {
            var username = email.Split('@')[0];

            if (UsernameExists(username))
            {
                username = username + "_" + Guid.NewGuid().ToString("N")[..6];
            }

            var password = Guid.NewGuid().ToString("N");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                ConfirmPassword = hashedPassword,
                Role = "user",
                IsBlocked = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

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

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public bool UsernameExists(string username)
        {
            return _context.Users
                .Any(x => x.Username == username);
        }

        public bool EmailExists(string email)
        {
            return _context.Users
                .Any(x => x.Email == email);
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(x => x.Email == email);
        }

        public bool ResetPassword(string email, string newPassword)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return false;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            user.Password = hashedPassword;
            user.ConfirmPassword = hashedPassword;

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