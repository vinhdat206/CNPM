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
            // hash password
            user.Password =
                BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);

            _context.SaveChanges();
        }

        // ================= LOGIN =================

        public User Login(
            string username,
            string password)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Username == username);

            if (user == null)
                return null;

            bool check =
                BCrypt.Net.BCrypt.Verify(
                    password,
                    user.Password);

            return check ? user : null;
        }

        // ================= CHECK USERNAME =================

        public bool UsernameExists(string username)
        {
            return _context.Users
                .Any(x => x.Username == username);
        }
    }
}