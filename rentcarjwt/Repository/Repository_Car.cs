using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Model;
using rentcarjwt.Model.Car_;
using Microsoft.EntityFrameworkCore;
using rentcarjwt.Model.Data;
using AutoMapper;
using rentcarjwt.Model.Car;
using rentcarjwt.Services.Mail;

namespace rentcarjwt.Repository
{
    public class Repository_Car: IRepository_Car
    {
        private readonly DataContext _context;
        private readonly IRepository_User repository_User;
        private readonly IMailService mailService;
        private readonly IRepository_Reserve repository_Reserve;
  

        public Repository_Car(DataContext context, IRepository_User _repository_User, IMailService _mailService, IRepository_Reserve _repository_Reserve)
        {
            _context = context;
            repository_User= _repository_User;
            mailService= _mailService;
            repository_Reserve= _repository_Reserve;
         
        }

      
      
        public async Task<Car> getCar(Guid id)
        {
            return await _context.Cars.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Car>> getCars()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<CarResponse> CreateNewCar(CarRequest carRequest, string usermail)
        {
            Car car = newCar(carRequest, usermail);
            _context.Add(car);
            await _context.SaveChangesAsync();
            return newCarResponse(car);

        }

        //використання  MapperConfiguration
        public async Task<List<CarResponse>> getListCar(string email)
        {
            List<Car> cars = await _context.Cars.Where(car => car.UserEmail == email && car.deleteDt == null).ToListAsync();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarResponse>()).CreateMapper();
            var mappeer = mapper.Map<List<Car>, List<CarResponse>>(cars);
            return mappeer;
        }
        public async Task<List<TenantCars>> Reserve(string idCar,string tenantEmail)
        {
            Car car= await getCar(new Guid(idCar));
            if (car.status == StatusCar.Order)
                return null;

            await mailService.SendEmailConfirmationRent(car, car.UserEmail, tenantEmail);// відправка повідомлення 
            car.status = StatusCar.Order;
            _context.Update(car);
           

           

                MarkerCar markerCar = await _context.MarkerCars.FirstOrDefaultAsync(p => p.IdCar == new Guid(idCar));// видалення маркеру
                 _context.MarkerCars.Remove(markerCar);
                 await _context.SaveChangesAsync();
                

            User user = await repository_User.CheckEmail(tenantEmail);
            await repository_Reserve.CreateNewReserve(car, user);

            List<TenantCars> result = await getMyReserve(tenantEmail);


            return result;
        }
        public async Task<List<TenantCars>> CancelReservation(string idCar,string tenantEmail)
        {
            Car car= await getCar(new Guid(idCar));
            await mailService.SendEmailCancelRent(car, car.UserEmail, tenantEmail);
            car.status = StatusCar.free;
            _context.Update(car);
            await _context.SaveChangesAsync();

            User user = await repository_User.CheckEmail(tenantEmail);
            await repository_Reserve.DeleteReserve(car, user);

            List<TenantCars> result = await getMyReserve(tenantEmail);
            return result;
        } 
        public async Task<CarResponse> ConfirmRent(string idCar)
        {
            Car car= await getCar(new Guid(idCar));
         
            car.status = StatusCar.busy;
            _context.Update(car);
            await _context.SaveChangesAsync();

            return  newCarResponse(car);
        } 
        public async Task<CarResponse> CancelmRentLessor(string idCar)
        {
            Car car= await getCar(new Guid(idCar));
         
            car.status = StatusCar.free;
            _context.Update(car);
            await _context.SaveChangesAsync();

            return  newCarResponse(car);
        }
        public async Task<List<TenantCars>> getMyReserve(string tenantEmail)
        {

            User user = await repository_User.CheckEmail(tenantEmail);

            List<Car> car =  _context.ReserveCars.Where(rc => rc.User.Id == user.Id).Select(rc => rc.Car).ToList();
            List<TenantCars> result = new List<TenantCars>();

            for(int i=0;i<car.Count;i++)
            {
                TenantCars temp= new TenantCars();
                temp.IdCar= car[i].Id;
                temp.price = car[i].price;
                temp.foto= car[i].foto;
                temp.UserEmail= car[i].UserEmail;
                temp.brand= car[i].brand;
                temp.model= car[i].model;
                temp.status = car[i].status;

                result.Add(temp);
            }
     

     
            return result;
        }
        private Car newCar(CarRequest carRequest, string usermail)
        {
            Car car = new Car();
            car.Id = Guid.NewGuid();
            car.UserEmail= usermail;
            car.model=carRequest.Model;
            car.brand=carRequest.Brand;
            car.climate=carRequest.Climate;
            car.door=carRequest.Door;
            car.color=carRequest.Color;
            car.price=carRequest.Price;
            car.age=carRequest.Age;
            car.numberOfSeats = carRequest.numberOfSeats;
            car.engineCapacity = carRequest.engineCapacity;
            car.transmissionType=carRequest.transmissionType;
            car.fuelEype = carRequest.fuelЕype;
            car.foto = carRequest.Foto;
            car.dateСreation= DateTime.Now;
            car.status = StatusCar.free;
            return car;
        }
        private CarResponse newCarResponse(Car car)
        {
            CarResponse carResponse = new CarResponse();
            carResponse.id = car.Id;
            carResponse.Model = car.model;
            carResponse.Brand = car.brand;
            carResponse.Climate = car.climate;
            carResponse.Door = car.door;
            carResponse.Color = car.color;
            carResponse.Price = car.price;
            carResponse.Age = car.age;
            carResponse.numberOfSeats = car.numberOfSeats;
            carResponse.engineCapacity = car.engineCapacity;
            carResponse.transmissionType = car.transmissionType;
            carResponse.fuelЕype = car.fuelEype;
            carResponse.Foto = car.foto;
            carResponse.DateCreation = car.dateСreation;
            carResponse.status = car.status;
            return carResponse;
        }

        public async Task  DeleteCar(string idcar)
        {
            try
            {
                Car car = await getCar(new Guid(idcar));
                car.status = StatusCar.free;
                car.deleteDt = DateTime.Now;


                MarkerCar markerCar = await _context.MarkerCars.FirstOrDefaultAsync(p => p.IdCar == new Guid(idcar));
                ReserveCar reserveCar = await _context.ReserveCars.FirstOrDefaultAsync(p => p.Car.Id == car.Id);
                if(reserveCar != null)
                    _context.ReserveCars.Remove(reserveCar); 
                if(markerCar!= null)
                    _context.MarkerCars.Remove(markerCar);
             
                _context.Cars.Update(car);
           
                await _context.SaveChangesAsync();
            }
            catch { }
          
           
        }
    }
    
}



