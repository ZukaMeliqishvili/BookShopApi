using BookShopApi.Dto._Category;
using FluentValidation;

namespace BookShopApi.Infrastructure.FluentValidations
{
    public class CategoryDtoValidator:AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50);
        }
    }
}
