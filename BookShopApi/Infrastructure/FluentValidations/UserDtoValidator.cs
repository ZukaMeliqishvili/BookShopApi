using BookShopApi.Dto.User;
using FluentValidation;

namespace BookShopApi.Infrastructure.FluentValidations
{
    public class UserDtoValidator:AbstractValidator<UserRegisterDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.UserName)
               .NotNull()
               .NotEmpty()
               .MinimumLength(4)
               .MaximumLength(20)
               .Matches("^[A-Za-z0-9]*$");
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$");
            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Matches("^5\\d{8}$");
            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(250);
        }
    }
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(25)
                .Matches("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$");
            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Matches("^5\\d{8}$");
            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(250);
        }
    }

}
