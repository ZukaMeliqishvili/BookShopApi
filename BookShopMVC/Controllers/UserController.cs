using BookShopMVC.Models;
using BookShopMVC.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BookShopMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string baseUrl;
        public UserController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            baseUrl = configuration["ApiBaseURL:url"];
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var url = baseUrl + "/Auth/login";
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse).Token;

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenPayload = tokenHandler.ReadJwtToken(token);
                var claims = tokenPayload.Claims;


                var usernameClaim = claims.FirstOrDefault(c => c.Type == "unique_name");

                var username = usernameClaim != null ? new Claim(ClaimTypes.Name, usernameClaim.Value) : null;
                var roleClaims = claims.Where(c => c.Type == "role").Select(c => new Claim(ClaimTypes.Role, c.Value)).ToList();

                claims = claims.Except(new[] { usernameClaim }).Concat(new[] { username,}).Concat(roleClaims);


                var claimsIdentity = new ClaimsIdentity(claims, "jwt");


                await HttpContext.SignInAsync("Identity.Application", new ClaimsPrincipal(claimsIdentity));
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(1)
                });

                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string url = baseUrl + "/Auth/register";
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await HttpContext.SignOutAsync();
           Response.Cookies.Delete("JwtToken");
           return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string url = baseUrl + "/User";
            var client = _clientFactory.CreateClient();
            var jwtToken = Request.Cookies["JwtToken"];
            if(jwtToken == null)
            {
                return RedirectToAction("Index","Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserResponseModel>>(jsonResponse);
                return View(users);
            }
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> MakeStaffUser(int id)
        {
            string url = baseUrl + $"/User/{id}";
            var client = _clientFactory.CreateClient();
            var jwtToken = Request.Cookies["JwtToken"];
            if(jwtToken == null)
            {
                return RedirectToAction("Index", "Home");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            //var content = new StringContent("");
            var response = await client.PutAsync(url,null);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update()
        {
            var url = baseUrl + "/User/UserInfo";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await client.GetAsync(url);
            if(!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserResponseModel>(jsonString);
            return View(user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(UserResponseModel user)
        {
            int emailErrors = ModelState["Email"].Errors.Count;
            int phoneNumberErrors = ModelState["PhoneNumber"].Errors.Count;
            int addressErrors = ModelState["Address"].Errors.Count;

            if (emailErrors > 0 || phoneNumberErrors>0 || addressErrors >0)
            {
                TempData["error"] = "Please enter valid info";
                return RedirectToAction("Update", "User");
            }
            UserUpdateModel model = new UserUpdateModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };
            var url = baseUrl + "/User";
            var jwtToken = Request.Cookies["JwtToken"];
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            if(!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["error"] = responseContent;
            }
            else
            {
                TempData["success"] = "Contact information has been updated successfully";
            }
            return RedirectToAction("Update", "User");
        }
    }
}
