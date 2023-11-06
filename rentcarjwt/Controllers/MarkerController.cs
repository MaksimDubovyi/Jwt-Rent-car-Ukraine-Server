using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using rentcarjwt.Model.Car_;
using rentcarjwt.Repository;
using rentcarjwt.Services.JwtToken;
using rentcarjwt.Model.Marker;
using rentcarjwt.Model;

namespace rentcarjwt.Controllers
{
    [Route("api/Marker")]
    [ApiController]
    public class MarkerController : Controller
    {

        private readonly IJwtToken jwtToken;
        private readonly IRepository_MarkerCar markerCar;
        public MarkerController( IJwtToken _jwtToken, IRepository_MarkerCar _markerCar)
        {

            jwtToken = _jwtToken;
            markerCar = _markerCar;
        }



        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MarkerRequest markerRequest)
        {
            if (!ModelState.IsValid)        // Перевірка наявності помилок у моделі запиту
                return BadRequest(markerRequest);

            string email = jwtToken.getEmailUser(markerRequest.accessToken);
            MarkerResponse result = await markerCar.Create(markerRequest, email);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("remove-from-map")]
        public async Task<IActionResult> RemoveFromMap([FromQuery] string idCar)
        {

           await markerCar.RemoveFromMap(idCar);

            return Ok();
        }

        [HttpPost("get-all-markers")]
        public async Task<IActionResult> getAllMarkers()
        {
            List<MarkerResponse> result=new List<MarkerResponse> ();
            try
            {
                 result = await markerCar.GetMarkerList();
                if(result==null)
                {
                    return BadRequest("---getAllMarkers BadRequest---");
                }
            }
            catch (Exception ex) { }


           

            return Ok(result);
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromBody] Filter filtermodel)
        {
            if (!ModelState.IsValid)             // Перевірка наявності помилок у моделі запиту
                return BadRequest(filtermodel);
            List<MarkerResponse> result = new List<MarkerResponse>();
            result = await markerCar.FiltertMarkerList(filtermodel);

            return Ok(result);
        }
        
    }
}
