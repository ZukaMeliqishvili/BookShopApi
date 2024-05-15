using BookShopApi.Dto.User;
using BookShopApi.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        protected virtual string GetUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task AssignStaffRole(int id)
        {
           await _userService.AssignStaffRoleToUser(id);
        }
        [Authorize]
        [HttpPut]
        public async Task UpdateUserInfo(UserUpdateDto dto)
        {
            await _userService.Update(int.Parse(GetUserId()),dto);
        }
        [Authorize]
        [HttpGet("UserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userService.GetUserInfo(int.Parse(GetUserId()));
            return Ok(user);
        }

    }
}
