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

        protected virtual string GetUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Authorize(Roles ="2")]
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            int userId = int.Parse(GetUserId());
            return Ok(await _orderService.GetUserOrders(userId));
        }
        [Authorize(Roles ="1")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await _orderService.GetOrders());
        }
        [Authorize(Roles = "2")]
        [HttpPost("{id}")]
        public async Task<IActionResult> MakeOrder(int id, OrderRequestDto dto)
        {
            try
            {
                await _orderService.MakeOrder(id, int.Parse(GetUserId()), dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
