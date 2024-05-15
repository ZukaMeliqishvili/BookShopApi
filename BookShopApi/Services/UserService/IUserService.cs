using BookShopApi.Dto.User;

namespace BookShopApi.Services.UserServices
{
    public interface IUserService
    {
        public Task Register(UserRegisterDto userRegisterDto);
        public Task<(int,string)> LogIn(UserLoginDto dto);
        public Task<List<UserResponseDto>> GetUsers();
        public Task AssignStaffRoleToUser(int userId);
        public Task Update(int userId, UserUpdateDto userUpdateDto);
        public Task<UserResponseDto> GetUserInfo(int userId);
    }
}
