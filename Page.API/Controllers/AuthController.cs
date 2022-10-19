using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Page.API.Models.Authentication;
using Page.API.Repository;
using Page.API.Services.AuthService;
using System.Security.Claims;

namespace Page.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(Register request)
        {
            var user = await _authService.Register(new User { Email = request.Email }, request.Password);

            if (!user)
            {
                return BadRequest(new Response(false, "User already exist"));
            }

            return Ok(new Response(true, "User successfully registered"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Login request)
        {
            var response = await _authService.Login(request.Email, request.Password);

            if (response != "Ok")
            {
                return BadRequest(new Response(false, response));
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                });

            return Ok(new Response(true, "User successfully logged in"));
        }

        [HttpPost("updatePassword")]
        public async Task<ActionResult<bool>> UpdatePassword(UpdatePassword request)
        {
            var userId = _unitOfWork.User.GetUserId();

            var response = await _authService.UpdatePassword(userId, request.Password);

            if (!response)
            {
                return BadRequest(new Response(false, "User not found"));
            }

            return Ok(new Response(true, "Password successfully updated"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> VerifyEmail(string id)
        {
            var response = await _authService.VerifyEmail(id);

            if(response != "Ok")
            {
                return BadRequest(new Response(false, response));
            }

            return Ok(new Response(true, "Email address verified"));
        }

        [HttpPost("forgottenPassword")]
        public async Task<ActionResult> ForgottenPassword(string email)
        {
            User user = await _unitOfWork.User.GetUserByEmail(email);

            if(user == null)
            {
                return BadRequest(new Response(false, "User not found"));
            }

            var response = await _authService.ForgottenPassword(user);

            return Ok(new Response(true, response));
        }

        [HttpPost("verifyPassword/{id}")]
        public async Task<ActionResult> VerifyPassword(string id, string newPassword)
        {
            var response = await _authService.VerifyPassword(id, newPassword);

            if(response != "Ok")
            {
                return BadRequest(new Response(false, response));
            }

            return Ok(new Response(true, "Password has changed"));
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            var userClaims = User.Claims.Select(x => new Claim(x.Type, x.Value)).ToList();

            return Ok(userClaims);
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
