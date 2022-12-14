using DataLayer.DTO;
using DataLayer.Models;
using Infrastructure.IRepositories;
using Infrastructure.IServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IOnlineStoreRead<User> _onlineStoreRead;
        private readonly IOnlineStoreWrite<User> _onlineStoreWrite;

        public UserService(IOnlineStoreRead<User> onlineStoreRead, IOnlineStoreWrite<User> onlineStoreWrite)
        {
            _onlineStoreRead = onlineStoreRead;
            _onlineStoreWrite = onlineStoreWrite;
        }
        
        public async Task<User> AddUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User can not be empty.");
            }
            if ( await GetByEmail(user.Email) != null )
            {
                throw new ArgumentException($"User with the same email {user.Email} already exists.");
            }
            User u = new User(
                Guid.NewGuid(),
                NullOrEmptyStringChecker(user.Name),
                user.Email,
                NullOrEmptyStringChecker(user.Password)
                );
            return await _onlineStoreWrite.Add(u);
        }

        public async Task DeleteUser(string userId)
        {
            User user = await GetById(NullOrEmptyStringChecker(userId));
            await _onlineStoreWrite.Delete(user);
        }

        public async Task<List<User>> GetAll()
        {
            return await _onlineStoreRead.GetAll().ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            NullOrEmptyStringChecker(email);
            var result = await _onlineStoreRead.GetAll().FirstOrDefaultAsync(user=>user.Email == email);
            return result;
        }

        public async Task<User> GetById(string userId)
        {
            Guid id;
            Guid.TryParse(NullOrEmptyStringChecker(userId),out id);

            if (!Guid.TryParse(NullOrEmptyStringChecker(userId), out id))
            {
                throw new ArgumentException("Invalid User ID was provided.");
            }

            var result = await _onlineStoreRead.GetAll().FirstOrDefaultAsync(user => user.Id == id);

            if (result == null)
            {
                throw new ArgumentException($"User with ID:{userId}, does not exist.");
            }

            return result; 
        }

        public async Task<User> UpdateUser(User user)
        {
            User u = await GetById(user.Id.ToString());
            u = user;
            await _onlineStoreWrite.Update(u);
            return u;
        }

        private string NullOrEmptyStringChecker (string stringToCheck)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentNullException("Please fill all the user information.");
            }
            return stringToCheck;
        }
    }
}
