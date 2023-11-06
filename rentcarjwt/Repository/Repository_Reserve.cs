using Microsoft.EntityFrameworkCore;
using rentcarjwt.Model.Car;
using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Services.Mail;

namespace rentcarjwt.Repository
{
    public class Repository_Reserve: IRepository_Reserve
    {
        private readonly DataContext _context;


        public Repository_Reserve(DataContext context)
        {
            _context = context;
        }



        public async Task CreateNewReserve(Car car, User user)
        {
            ReserveCar reserveCar = new ReserveCar();
            reserveCar.Id= Guid.NewGuid();
            reserveCar.User= user;
            reserveCar.Car= car;
            _context.Add(reserveCar);
            await _context.SaveChangesAsync(); 
        }
        
        public async Task DeleteReserve(Car car, User user)
        {
            ReserveCar reserveCar = await getReserve(car, user);

            if(reserveCar != null) {
                _context.ReserveCars.Remove(reserveCar);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ReserveCar> getReserve(Car car, User user)
        {
            return await _context.ReserveCars.FirstOrDefaultAsync(p => p.Car.Id == car.Id && p.User.Id == user.Id);
        }

    }
}
