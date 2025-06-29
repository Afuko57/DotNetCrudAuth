using MyApiTest.Models;

namespace MyApiTest.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsAsync(string username);
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}