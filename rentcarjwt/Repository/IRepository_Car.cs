using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Model;
using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data;
using rentcarjwt.Model.Car;

namespace rentcarjwt.Repository
{
   

    public interface IRepository_Car
    {
        Task<CarResponse> CreateNewCar(CarRequest carRequest, string usermail);
        Task <List<CarResponse>> getListCar(string email);
        Task<Car> getCar(Guid id);
        Task<List<Car>> getCars();
        Task<List<TenantCars>> Reserve(string idCar, string tenantEmail);
        Task<List<TenantCars>> CancelReservation(string idCar, string tenantEmail);
        Task<List<TenantCars>> getMyReserve(string tenantEmail);
        Task<CarResponse> ConfirmRent(string tenantEmail);
        Task<CarResponse> CancelmRentLessor(string tenantEmail);
        Task DeleteCar(string idcar);
    }
    
}
