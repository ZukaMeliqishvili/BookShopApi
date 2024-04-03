using BookShopApi.Dto.User;

namespace BookShopApi.Services.UserServices
{
    public interface IUserService
    {
        public Task Register(UserRegisterDto userRegisterDto);
        public Task<(int,int)> LogIn(UserLoginDto dto);
    }
}
