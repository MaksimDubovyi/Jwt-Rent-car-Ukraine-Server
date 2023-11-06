using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Model.MessageModel;
using rentcarjwt.Repository;
using rentcarjwt.Services.JwtToken;

namespace rentcarjwt.Controllers
{
  
        [Route("api/Message")]
        [ApiController]
        public class MessageController : Controller
        {
            private readonly IRepository_Message repository_Message;
            private readonly IJwtToken jwtToken;
            public MessageController(IRepository_Message _repository_Message, IJwtToken _jwtToken)
            {
                repository_Message = _repository_Message;
                jwtToken = _jwtToken;
            }

            //[Authorize]
            //[HttpPost("send-message")]
            //public async Task<IActionResult> Send([FromQuery] string idCar, string token, string txt)
            //{
            //    string emailSender = jwtToken.getEmailUser(token);
            //    MessageResponse messageResponse = await repository_Message.sendMessage( emailSender,  idCar,  txt);
            //    return Ok(messageResponse);
            //}
  

        }
    }
