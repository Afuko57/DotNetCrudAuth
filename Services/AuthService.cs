using MyApiTest.Models;
using MyApiTest.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MyApiTest.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Register(string username, string password)
        {
            if (await _db.Users.AnyAsync(u => u.Username == username))
                return false;

            var hashed = Hash(password);
            var user = new User { Username = username, PasswordHash = hashed };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return false;

            return user.PasswordHash == Hash(password);
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
