﻿using BookShopApi.Dto.ShoppingCart;
using BookShopApi.Services.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        protected virtual string GetUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _shoppingCartService.GetAll((int.Parse(GetUserId()))));
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCartItemRequestDto item)
        {
            try
            {
                await _shoppingCartService.AddToCart(item,int.Parse(GetUserId()));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _shoppingCartService.RemoveFromCart(id, int.Parse(GetUserId()));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Roles ="User")]
        [HttpDelete("RemoveAll")]
        public async Task<IActionResult> RemoveAll()
        {
            await _shoppingCartService.RemoveAllItemsFromCart(int.Parse(GetUserId()));
            return Ok();
        }
    }
}
