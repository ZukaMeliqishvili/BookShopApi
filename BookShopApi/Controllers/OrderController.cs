using BookShopApi.Dto._Order;
using BookShopApi.Services._Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        protected string GetCurrencyCodeFromCookies()
        {
            string currencyCode = Request.Cookies["currencyCode"];
            if (String.IsNullOrEmpty(currencyCode))
            {
                currencyCode = "gel";
            }
            return currencyCode;
        }
        protected virtual string GetUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            int userId = int.Parse(GetUserId());
            return Ok(await _orderService.GetUserOrders(userId));
        }
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _orderService.GetOrders());
        }
        [Authorize(Roles = "User")]
        [HttpPost("{id}")]
        public async Task<IActionResult> MakeOrder(int id, OrderRequestDto dto)
        {
            try
            {
                await _orderService.MakeOrder(id, int.Parse(GetUserId()), dto, GetCurrencyCodeFromCookies());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
