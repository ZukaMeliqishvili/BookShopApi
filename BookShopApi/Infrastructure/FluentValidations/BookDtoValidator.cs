using BookShopApi.Dto._Book;
using FluentValidation;

namespace BookShopApi.Infrastructure.FluentValidations
{
    public class BookDtoValidator:AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(20)
                .MaximumLength(2000);
            RuleFor(x => x.Author)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);
            RuleFor(x => x.Price)
                .NotNull()
                .Must(x => x > 1 && x <= 1000)
                .WithMessage("Price must be higher than 1 and lower or equal than 1000");
            RuleFor(x => x.NumberOfPages)
                .NotNull()
                .Must(x => x > 5 && x < 10000)
                .WithMessage("Number of pages must be higher than 5 and lower or equal than 10000");
            RuleFor(x => x.AmountInStock)
                .NotNull()
                .Must(x => x > 0 && x < 100000)
                .WithMessage("Amount must be higher than 5 and lower or equal than 10000");
            RuleFor(x => x.CategoryIds)
                .NotNull()
                .NotEmpty();
        }
    }
    public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
    {
        public BookUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(20)
                .MaximumLength(2000);
            RuleFor(x => x.Author)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250)
                .WithMessage("Price must be higher than 1 and lower or equal than 1000");
            RuleFor(x => x.Price)
                .NotNull()
                .Must(x => x > 1 && x <= 1000);
            RuleFor(x => x.NumberOfPages)
                .NotNull()
                .Must(x => x > 5 && x < 10000)
                .WithMessage("Number of pages must be higher than 5 and lower or equal than 10000");
        }
    }
}
