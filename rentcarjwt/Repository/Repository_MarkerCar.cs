using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Model.Data;
using rentcarjwt.Model.Marker;
using Microsoft.EntityFrameworkCore;
using rentcarjwt.Model;
using System.Drawing;

namespace rentcarjwt.Repository
{
    public class Repository_MarkerCar : IRepository_MarkerCar
    {
        private readonly DataContext _context;
        private readonly IRepository_Car repository_Car;
        public Repository_MarkerCar(DataContext context, IRepository_Car _repository_Car)
        {
            _context = context;
            repository_Car = _repository_Car;
        }

        public async  Task<List<MarkerResponse>> GetMarkerList()
        {
            List<MarkerResponse> result = new List<MarkerResponse>();
            var markerCars = await _context.MarkerCars.ToListAsync();
            var cars = await _context.Cars.Where(c => c.status != StatusCar.Order ).ToListAsync();

            // Объединение данных из таблиц MarkerCar и Cars в модель MarkerResponse
            result = markerCars
                .Where(mc => cars.Any(c => c.Id == mc.IdCar)) // Фильтрация markerCars по наличию в cars
                .Select(markerCar => new MarkerResponse
                {
                    Id = markerCar.Id.ToString(),
                    IdCar = markerCar.IdCar.ToString(),
                    dateFrom = markerCar.fromDate,
                    dateTo = markerCar.toDate,
                    lat = markerCar.lat,
                    lng = markerCar.lng,
                    UserEmail = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.UserEmail,
                    foto = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.foto,
                    price = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.price,
                    brand = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.brand,
                    model = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.model,
                    age = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.age,
                    color = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.color
                })
                .ToList();


            return result;
        }

        public async Task<MarkerResponse> Create(MarkerRequest markerRequest, string usermail)
        {
            Car car = await repository_Car.getCar(new Guid(markerRequest.idCar));
            if(car.status== StatusCar.awaitingСonfirmation) { return null; }
            MarkerCar markerCar = new MarkerCar();
            
            markerCar.Id = Guid.NewGuid();
            markerCar.IdCar = new Guid(markerRequest.idCar);
            markerCar.fromDate = markerRequest.dateFrom;
            markerCar.toDate = markerRequest.dateTo;
            markerCar.lat=markerRequest.lat;
            markerCar.lng=markerRequest.lng;
            
            try
            {
                car.status = StatusCar.awaitingСonfirmation;
                _context.Cars.Update(car);
                _context.Add(markerCar);
                await _context.SaveChangesAsync();
            }
             catch(Exception ex) { }

            MarkerResponse markerResponse= new MarkerResponse();
            markerResponse.Id = markerCar.Id.ToString();
            markerResponse.IdCar = markerCar.IdCar.ToString();
            markerResponse.dateFrom = markerCar.fromDate;
            markerResponse.dateTo = markerCar.toDate;
            markerResponse.lat = markerCar.lat;
            markerResponse.lng = markerCar.lng;

            markerResponse.UserEmail = car.UserEmail;
            markerResponse.foto = car.foto;
            markerResponse.price = car.price;
            markerResponse.brand = car.brand;
            markerResponse.model = car.model;
            markerResponse.age = car.age;
            markerResponse.color= car.color;

            return markerResponse;
        }

        public async Task RemoveFromMap(string idCar)
        {
     
            Car car = new Car();
            MarkerCar markerCar = new MarkerCar();

            try
            {
                car = await _context.Cars.FirstOrDefaultAsync(p => p.Id == new Guid(idCar));
                markerCar = await _context.MarkerCars.FirstOrDefaultAsync(p => p.IdCar == new Guid(idCar));        
            }
            catch (Exception ex) { }
            if (car!=null&& markerCar!=null)
            {

                try
                {

                    car.status = StatusCar.free;
                    _context.Cars.Update(car);
                    _context.MarkerCars.Remove(markerCar);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { }
            }
        }

        public async Task<List<MarkerResponse>> FiltertMarkerList(Filter filter)
        {

            var query = _context.Cars.AsQueryable(); // Починаємо з усіх автомобілів 

            query = query.Where(c => c.status != StatusCar.Order);
            // Додаємо параметри до запиту
            if (filter.door!=0) // якщо користувач обрав фільтрацію за кількістю дверей
            {
                query = query.Where(c => c.door == filter.door);
            }

            if (filter.climate ==true) // Вибрати лише із кондиціонером
            {
                query = query.Where(c => c.climate == filter.climate);
            }


            if (filter.priceFrom !=null&& filter.priceTo != null)        // Вибрати лише які потрапляють діапазон ціни
            {
                query = query.Where(c => c.price >= filter.priceFrom&& c.price <= filter.priceTo);
            }
            else if(filter.priceFrom != null)                            // Якщо лише потрібно від указаної ціни     
            {
                query = query.Where(c => c.price >= filter.priceFrom);
            }
            else if(filter.priceTo != null)                              // Якщо лише потрібно до указаної ціни     
            {
                query = query.Where(c => c.price <= filter.priceTo);
            }


            if (filter.ageFrom != null && filter.ageTo != null)        // Вибрати лише які потрапляють діапазон року
            {
                query = query.Where(c => c.age >= filter.ageFrom && c.age <= filter.ageTo);
            }
            else if (filter.ageFrom != null)                            // Якщо лише потрібно від указаного року    
            {
                query = query.Where(c => c.age >= filter.ageFrom);
            }
            else if (filter.ageTo != null)                              // Якщо лише потрібно до указаного року    
            {
                query = query.Where(c => c.age <= filter.ageTo);
            }

            if (filter.numberOfSeats != null)                              // кількість місць    
            {
                query = query.Where(c => c.numberOfSeats == filter.numberOfSeats);
            }
            if (filter.transmissionType != null)                              // Тип трансмісії    
            {
                query = query.Where(c => c.transmissionType == filter.transmissionType);
            }
            if (filter.fuelЕype != null)                              // тип палива    
            {
                query = query.Where(c => c.fuelEype == filter.fuelЕype);
            }
            if (filter.brand != null)                              // марка    
            {
                query = query.Where(c => c.brand == filter.brand);
            }
            if (filter.model != null)                              // модель    
            {
                query = query.Where(c => c.model == filter.model);
            }


            if (filter.dateFrom != null&& filter.dateTo != null)
            { 
             // Отримуємо список IdCar з MarkerCar, які мають маркери на карті
              var carsWithMarkers = _context.MarkerCars                     // отримали всі маркери 
                .Where(markerCar => markerCar.fromDate >= filter.dateFrom&& markerCar.toDate<=filter.dateTo)// перевіряємо дати
                .Select(markerCar => markerCar.IdCar) // отримуємо вибірку з IdCar 
                .ToList();
              query = query.Where(car => carsWithMarkers.Contains(car.Id));  // Фільтруємо Car за Id, які знайдені в списку carsWithMarkers
            }
            else if (filter.dateFrom != null)
            {
                var carsWithMarkers = _context.MarkerCars
                  .Where(markerCar => markerCar.fromDate >= filter.dateFrom)
                  .Select(markerCar => markerCar.IdCar)
                  .ToList();
                query = query.Where(car => carsWithMarkers.Contains(car.Id));
            }
            else if (filter.dateTo != null)
            {
                var carsWithMarkers = _context.MarkerCars
                  .Where(markerCar => markerCar.toDate <= filter.dateTo)
                  .Select(markerCar => markerCar.IdCar)
                  .ToList();
                query = query.Where(car => carsWithMarkers.Contains(car.Id));
            }
            else 
            {
               var carsWithMarkers = _context.MarkerCars
                .Select(markerCar => markerCar.IdCar)
                .ToList();
                query = query.Where(car => carsWithMarkers.Contains(car.Id));
            }

            // Виконати запит до бази даних і поверніть результат
            List<Car> cars =  query.ToList();
            // Объединение данных из таблиц MarkerCar и Cars в модель MarkerResponse
            List<MarkerResponse> result = new List<MarkerResponse>();
            var markerCars = await _context.MarkerCars.ToListAsync();


            // Об'єднання даних із таблиць MarkerCar і Cars в модель MarkerResponse
            result = markerCars                                // Фільтруємо markerCars, залишаючи лише ті записи, для яких існує відповідний об'єкт в cars. 
                .Where(mc => cars.Any(c => c.Id == mc.IdCar))  // Це робиться за допомогою метода Any, який перевіряє, чи є хоча б один елемент в cars, який має Id, співставний з IdCar в markerCars.
                .Select(markerCar => new MarkerResponse    // Для кожного залишеного запису в markerCars, ми створюємо новий об'єкт MarkerResponse
                {                                         
                    Id = markerCar.Id.ToString(),         
                    IdCar = markerCar.IdCar.ToString(),    
                    dateFrom = markerCar.fromDate,          // Id, IdCar, dateFrom, dateTo, lat, lng
                    dateTo = markerCar.toDate,              // беруться з відповідних властивостей markerCar.
                    lat = markerCar.lat,                   
                    lng = markerCar.lng,
                    UserEmail = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.UserEmail, // UserEmail, foto, price, brand, model, age, color беруться з 
                    foto = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.foto,           // відповідних властивостей cars, за умови, що знайдено
                    price = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.price,         // відповідний об'єкт Car з відповідним Id. Якщо такого об'єкта не знайдено, ці властивості будуть null.
                    brand = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.brand,         // ці властивості будуть null.
                    model = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.model,        
                    age = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.age,            
                    color = cars.FirstOrDefault(c => c.Id == markerCar.IdCar)?.color         
                })                                                                           
                .ToList();

            return result;
        }
  
    }
}
