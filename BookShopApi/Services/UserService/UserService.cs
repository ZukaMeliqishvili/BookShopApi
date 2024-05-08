using BookShopApi.Dto.User;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;
using System.Security.Cryptography;
using System.Text;

namespace BookShopApi.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<(int,string)> LogIn(UserLoginDto dto)
        {
            var user = await _userRepository.GetUser(dto.UserName);
            if (user == null)
            {
                throw new Exception("incorect user Credentials");
            }
            if (user.PasswordHash != HashPassword(dto.Password))
            {
                throw new Exception("incorect user Credentials");
            }
            
            return (user.Id,user.Role.Name);
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            bool userExists = await _userRepository.Exists(userRegisterDto.UserName);
            if (userExists)
            {
                throw new Exception("User Already Exists");
            }
            string passwordHash = HashPassword(userRegisterDto.Password);
            var user = userRegisterDto.Adapt<User>();
            user.PasswordHash = passwordHash;
            await _userRepository.Insert(user);

        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                password += "SmokeOnTheWater";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
