using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookShopApi.Dto._Order
{
    public class OrderRequestDto
    {
        [Range(1, 100)]
        public int Quantity { get; set; }
        public string Currency { get; set; }
    }
}
