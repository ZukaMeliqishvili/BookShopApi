using BookShopApi.Services._Order;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost]
        public async Task<IActionResult> MakeOrder()
        {
            try
            {
                await _orderService.MakeOrder(int.Parse(GetUserId()), GetCurrencyCodeFromCookies());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("userOrder/{id}")]
        public async Task<IActionResult> GetUserOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrder(id, int.Parse(GetUserId()));
                return Ok(order);
            }
            catch
            {
                return NotFound();
            }
        }
        [Authorize(Roles ="Admin,Staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrder(id);
                return Ok(order);
            }
            catch
            {
                return NotFound();
            }
        }
        [Authorize(Roles ="Admin,Staff")]
        [HttpPut("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeOrderStatus(int id)
        {
            try
            {
                await _orderService.ProceedOrder(id);
                return Ok();
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
