using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using rentcarjwt.Model;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using rentcarjwt.Model.Car_;
using Jose;
using Azure.Core;
using rentcarjwt.Services.JwtToken;
using rentcarjwt.Model.UserModel;

namespace rentcarjwt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository_User repository_User;
        private readonly IJwtToken jwtToken;
        public AuthController(IJwtToken _jwtToken, IRepository_User _repository_User)
        {
            repository_User= _repository_User;
            jwtToken= _jwtToken;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            
            if (!ModelState.IsValid)        // Перевірка наявності помилок у моделі запиту
                return BadRequest(request);


            User user = await repository_User.CheckEmail(request.email);
            // Перевірка, чи користувач з таким email вже існує
            if (user != null)
            {
                var errorResponse = new
                {
                    ErrorCode = "412",
                    ErrorMessage = "Mail is used!"
                };
                return BadRequest(errorResponse);
            }

            // Додавання нового користувача
            user = await repository_User.AddUser(request);

            // Перевірка, чи користувач був успішно створений
            if (user != null)
            {
                // Якщо користувач успішно створений, то спробуємо автоматично увійти
                return Ok(user);
            }
            else
            {
                // Якщо сталася помилка при створенні користувача
                return BadRequest("Error during creation");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest userAuth)
        {
            // Перевірка, чи дані користувача є вірними
            if (!ModelState.IsValid)
                return BadRequest(userAuth); // В разі невірних даних, повертаємо статус "BadRequest" і об'єкт "userAuth" для відображення помилок

            // Перевірка, чи існує користувач з вказаним email
            User user = await repository_User.CheckEmail(userAuth.email);
            if (user == null)
                return BadRequest("Bad credentials");// Якщо користувача з таким email не знайдено, повертаємо статус "BadRequest" з повідомленням "Bad credentials"

            // Перевірка пароля користувача
            string password = repository_User.CheckPassword(userAuth.password, user.salt);
            if (user.passwordHash != password)
                return BadRequest("Bad credentials");// Якщо пароль невірний, також повертаємо статус "BadRequest" з повідомленням "Bad credentials"
            
            // Генерація JWT-токену для аутентифікованого користувача
            string token = jwtToken.GetToken(user.email,user.firstName);

            // Генерація та оновлення refresh token
            user.RefreshToken = jwtToken.GenerateRefreshToken();
            DateTime currentDateTime = DateTime.Now;
            DateTime refreshTokenExpiryTime = currentDateTime.AddDays(30);
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await repository_User.UpdateUser(user);
            // Повертаємо успішний результат аутентифікації відповідно до AuthResponse
            return Ok(new AuthResponse
            {
                firstName = user.firstName,
                email = user.email,
                refreshToken = user.RefreshToken,
                accessToken = token,
                emailConfirmCode =user.emailConfirmCode,
                avatar = user.avatar
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
        {
            // Перевірка, чи передані дані для оновлення токенів
            if (tokenModel is null)
            return BadRequest("Invalid client request");

            // Отримання доступу та оновлення токенів з запиту
            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;/////////////////////////*********************

            // Отримання Principal (представлення клеймів) з доступного токену
            var principal = jwtToken.GetPrincipalFromExpiredToken(accessToken);

            // Перевірка наявності та валідності Principal
            if (principal == null)
            {
                var errorResponse = new
                {
                    ErrorCode = "403",
                    ErrorMessage = "Invalid access token or refresh token"
                };
                return BadRequest(errorResponse);
            }

            // Отримання email з Principal
            string email = jwtToken.getMailFromToken(principal);

            // Перевірка наявності користувача за email, відповідності refresh token і часу його дії
            User user = await repository_User.CheckEmail(email);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return BadRequest("Invalid access token or refresh token");

            // Створення нового доступу та оновлення токенів
            var newAccessToken = jwtToken.CreateToken(principal.Claims.ToList());
            var newRefreshToken = jwtToken.GenerateRefreshToken();

            // Оновлення refresh token користувача в базі даних
            user.RefreshToken = newRefreshToken;
            await repository_User.UpdateUser(user);

            // Повернення нового токену відповідно до результатів оновлення
      
            return new ObjectResult(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromQuery] string email)
        {
            User user = await repository_User.CheckEmail(email);
            if (user == null) return BadRequest("Invalid user email");

            user.RefreshToken = null;
            await repository_User.UpdateUser(user);

            return Ok();
        }

   
        [HttpPost("confirmMail")]
        public async Task<IActionResult> ConfirmMail([FromBody] UserShow code)
        {
            User user = await repository_User.CheckEmail(code.email);
            if (user == null) return BadRequest("Invalid user email");

            if (code.emailConfirmCode == user.emailConfirmCode)
            {
                user.emailConfirmCode = null;
                await repository_User.UpdateUser(user);
                return Ok(user);
            }

            return BadRequest("Invalid user email");
        }
        
    } 
}



