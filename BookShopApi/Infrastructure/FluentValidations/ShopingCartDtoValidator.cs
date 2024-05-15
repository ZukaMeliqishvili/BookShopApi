using BookShopApi.Dto.ShoppingCart;
using FluentValidation;
using Mapster;

namespace BookShopApi.Infrastructure.FluentValidations
{
    public class ShopingCartDtoValidator:AbstractValidator<ShoppingCartItemRequestDto>
    {
        public ShopingCartDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .NotNull()
                .Must(x => x > 1 && x <= 1000);
        }
    }
}
