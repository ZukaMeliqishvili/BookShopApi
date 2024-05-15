using BookShopApi.Dto.ShoppingCart;
using BookShopApi.Entities;
using BookShopApi.Repository;
using Mapster;

namespace BookShopApi.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly MyDapper _myDapper;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IBookRepository bookRepository, MyDapper myDapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _bookRepository = bookRepository;
            _myDapper = myDapper;
        }

        public async Task AddToCart(ShoppingCartItemRequestDto item,int userId)
        {
            var book = await _bookRepository.GetById(item.BookId);
            if(book == null)
            {
                throw new Exception("Book was not found");
            }
            var cartItem = await _shoppingCartRepository.GetByBookId(item.BookId, userId);
            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
               await _shoppingCartRepository.SaveChangesAsync();
                return;
            }
            if(book.AmountInStock < item.Quantity)
            {
                throw new Exception("There is not enough books in stock");
            }
            var entity = new ShoppingCartItem()
            {
                Book = book,
                BookId = item.BookId,
                Quantity = item.Quantity,
                UserId = userId
            };
            await _shoppingCartRepository.Add(entity);
        }

        public async Task<List<ShoppingCartItemResponseDto>> GetAll(int userId)
        {
            return (await _shoppingCartRepository.GetAll(userId)).Adapt<List<ShoppingCartItemResponseDto>>();
        }

        public async Task RemoveAllItemsFromCart(int userId)
        {
            await _myDapper.RemoveAllItemsFromCart(userId);
        }

        public async Task RemoveFromCart(int id,int userId)
        {
            var cartItem =await  _shoppingCartRepository.Get(id, userId);
            if(cartItem ==null)
            {
                throw new Exception("Item was not found");
            }
            await _shoppingCartRepository.Delete(cartItem);
        }
    }
}
