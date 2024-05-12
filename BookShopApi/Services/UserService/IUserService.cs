using BookShopApi.Dto.User;

namespace BookShopApi.Services.UserServices
{
    public interface IUserService
    {
        public Task Register(UserRegisterDto userRegisterDto);
        public Task<(int,string)> LogIn(UserLoginDto dto);
        public Task<List<UserRequestDto>> GetUsers();
        public Task AssignStaffRoleToUser(int userId);
    }
}
