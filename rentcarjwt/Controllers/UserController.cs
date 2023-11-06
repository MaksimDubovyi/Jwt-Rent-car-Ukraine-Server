
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using rentcarjwt.Model;
using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Repository;
using rentcarjwt.Services.JwtToken;
using rentcarjwt.Services.Mail;

namespace rentcarjwt.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IRepository_User repository_User;
        private readonly IJwtToken jwtToken;
        private readonly IMailService mailService;
        public UserController(IJwtToken _jwtToken, IRepository_User _repository_User, IMailService _mailService)
        {
            repository_User = _repository_User;
            jwtToken = _jwtToken;
            mailService = _mailService;
        }


        [Authorize]
        [HttpPost("update-password")]
        public async Task<IActionResult> updatePassword([FromQuery] string password, string accessToken)
        {
            var principal = jwtToken.GetPrincipalFromExpiredToken(accessToken);
            string email = jwtToken.getMailFromToken(principal);
            User user = await repository_User.CheckEmail(email);
            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                await repository_User.UpdatePassword(password, user);
                return Ok(new { StatusCode = "200" });
            }
            catch (Exception ex) { }
            return BadRequest();

           
        }
       

        [HttpPost("send-new-password")]
        public async Task<IActionResult> sendNewPassword(string email)
        {

            User user = await repository_User.CheckEmail(email);
            if (user == null)
            {
                return BadRequest();
            }
            try
            {
              string password=  await repository_User.GeneratePassword(user);
              await mailService.SendEmailNewPassword(password, user.email);
                return Ok(new { StatusCode = "200" });
            }
            catch (Exception ex) { }
            return BadRequest();

           
        }

        [Authorize]
        [HttpPost("get-user")]
        public async Task<IActionResult> getUser(string accessToken)
        {
            var principal = jwtToken.GetPrincipalFromExpiredToken(accessToken);
            string email = jwtToken.getMailFromToken(principal);
            User user = await repository_User.CheckEmail(email);
            if (user == null)
                return BadRequest();
            AuthResponse authResponse = new AuthResponse();
            authResponse.firstName = user.firstName;
            authResponse.avatar = user.avatar;
            authResponse.email = user.email;
            authResponse.emailConfirmCode = user.emailConfirmCode;
                return Ok(authResponse);
        
        }

        [Authorize]
        [HttpPost("update-avatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarModel userRequest)
        {
            string email = jwtToken.getEmailUser(userRequest.Email);
            if (email == null) { return BadRequest("400"); }
            User user = await repository_User.CheckEmail(email);
            if (user == null)
                return BadRequest("Bad credentials");// Якщо користувача з таким email не знайдено, повертаємо статус "BadRequest" з повідомленням "Bad credentials"
            user.avatar = userRequest.FileName;
            await repository_User.UpdateUser(user);
            AuthResponse authResponse=new AuthResponse();
            authResponse.firstName = user.firstName;
            authResponse.email = user.email;
            authResponse.avatar= user.avatar;
            return Ok(authResponse);
        }
        public class UpdateAvatarModel
        {
            public string Email { get; set; }
            public string FileName { get; set; }
        }
    }
}
