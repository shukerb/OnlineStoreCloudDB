using DataLayer.Models;
using DataLayer.DTO;

namespace Infrastructure.IServices
{
    public interface IUserService
    {
        Task<User> GetById(string userId);
        Task<List<User>> GetAll();
        Task<User> AddUser(UserDTO user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(string userId);
    }
}
