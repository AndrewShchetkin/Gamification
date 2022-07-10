using Gamification.Data;
using Gamification.Models.DTO;
using Gamification.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Gamification.Models.Errors;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Controllers
{
    [Route(template: "api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       private readonly IUserRepository _userRepository;
        private readonly ApplicationContext _context;
       public AuthController(IUserRepository userRepository, ApplicationContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }


        [HttpPost(template: "register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            var user = _userRepository.GetUserByUserName(userDto.UserName);
            // если пользователь с таким именем уже существует
            if (user != null)
            {
                var errorResponse = new ErrorResponse();
                var error = new ErrorModel
                {
                    FieldName = "UserName",
                    Message = "Пользователь с таким именем уже существует"
                };
                errorResponse.Errors.Add(error);
                return BadRequest(errorResponse);
            }

            user = new User { UserName = userDto.UserName,Password = BCrypt.Net.BCrypt.HashPassword( userDto.Password) };
            Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
            if (userRole != null)
                user.Role = userRole;

            await _userRepository.Create(user);

            await AuthenticateAsync(user);

            return Created("success", new { msg = "ok!" }); 
            // здесь в Created был объект user, но с ним 500 ошибка NewtonJson цикл
        }
        
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login(UserRegisterDto userDto)
        {
            var user = _userRepository.GetUserByUserName(userDto.UserName);
            // если нет такого пользователя в базе
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid Credentials" });
            }
            // если пароль не прошел верификацию 
            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid Credentials" });
            }

            await AuthenticateAsync(user);

            Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            return Ok(new { message = "success", userName = user.UserName , userTeamId = user.TeamId, userRole = userRole.Name});
        }
        
        [HttpPost(template: "logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        
        [HttpGet(template: "user")]
        public async Task<IActionResult> GetUser()
        {
            if(string.IsNullOrEmpty(User.Identity.Name))
                return Unauthorized(new { message = "Invalid Credentials" });

            var user = _userRepository.GetUserByUserName(User.Identity.Name);
            if (user == null)
            {
                return BadRequest("Пользователь не найден");
            }

            Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            return Ok(new {userName = User.Identity.Name, userTeamId = user.TeamId, userRole=userRole.Name });
        }

        // Helpers methods
        private async Task AuthenticateAsync(User user)
        {
            Role userRole = await _context.Roles.FirstOrDefaultAsync(role => role.Id == user.RoleId);

            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
