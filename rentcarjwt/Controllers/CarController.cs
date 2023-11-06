using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using rentcarjwt.Model.Car;
using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Repository;
using rentcarjwt.Services.JwtToken;
using System.Collections.Generic;

namespace rentcarjwt.Controllers
{
    [Route("api/Car")]
    [ApiController]
    public class CarController : Controller
    {
        private readonly IRepository_Car repository_Car;
        private readonly IJwtToken jwtToken;
        public CarController(IRepository_Car _repository_Car, IJwtToken _jwtToken)
        {
            repository_Car= _repository_Car;
            jwtToken= _jwtToken;
        }

        [Authorize]
        [HttpPost("get-car")]
        public async Task<IActionResult> GetCar([FromQuery] string idCar)
        {
            Car car = await repository_Car.getCar(new Guid(idCar));
            return Ok(car);
        }

        [HttpGet("get-cars")]
        public async Task<IActionResult> GetCars()
        {
            List <Car> car = await repository_Car.getCars();
            return Ok(car);
        }



        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CarRequest carRequest)
        {
            if (!ModelState.IsValid)        // Перевірка наявності помилок у моделі запиту
                return BadRequest(carRequest);

            CarResponse carResponse= await repository_Car.CreateNewCar(carRequest, jwtToken.getEmailUser(carRequest.AccessToken));
            return Ok(carResponse);
        }

        [Authorize]
        [HttpPost("get-my-car")]
        public async Task<IActionResult> getListCar(string accessToken)
        {

            List<CarResponse> result = await repository_Car.getListCar(jwtToken.getEmailUser(accessToken));
            return Ok(result);
        }



        [Authorize]
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromQuery] string idCar, string tenantEmail)
        {
            string tenant_email = jwtToken.getEmailUser(tenantEmail);
            List <TenantCars> tenantCars= await repository_Car.Reserve(idCar, tenant_email);
            return Ok(tenantCars);
        }


        [Authorize]
        [HttpPost("cancel-reservation")]
        public async Task<IActionResult> CancelReservation([FromQuery] string idCar, string tenantEmail)
        {
            string tenant_email = jwtToken.getEmailUser(tenantEmail);
            List<TenantCars> tenantCars = await repository_Car.CancelReservation(idCar, tenant_email);
            return Ok(tenantCars);
        }
        [Authorize]
        [HttpPost("confirm-rent")]
        public async Task<IActionResult> ConfirmRent([FromQuery] string idCar)
        {
            CarResponse carResponse = await repository_Car.ConfirmRent(idCar);
            return Ok(carResponse);
        }  
        [Authorize]
        [HttpPost("cancelm-rent-lessor")]
        public async Task<IActionResult> CancelmRentLessor([FromQuery] string idCar)
        {
            CarResponse carResponse = await repository_Car.CancelmRentLessor(idCar);
            return Ok(carResponse);
        }


        [Authorize]
        [HttpPost("get-my-reserve")]
        public async Task<IActionResult> getMyReserve([FromQuery] string tenantEmail)
        {
            string tenant_email = jwtToken.getEmailUser(tenantEmail);
            List <TenantCars> tenantCars= await repository_Car.getMyReserve(tenant_email);
            return Ok(tenantCars);
        }



        [Authorize]
        [HttpPost("delete-car")]
        public async Task<IActionResult> DeleteCar([FromQuery]  string idCar, string accessToken)
        {
            await repository_Car.DeleteCar(idCar);
            
            List<CarResponse> result = await repository_Car.getListCar(jwtToken.getEmailUser(accessToken));
            return Ok(result);
        }

    }
}
