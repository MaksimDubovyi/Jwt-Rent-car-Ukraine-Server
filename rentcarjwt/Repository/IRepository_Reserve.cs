using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Repository
{
    public interface IRepository_Reserve
    {
        Task CreateNewReserve(Car car, User user);
        Task DeleteReserve(Car car, User user);
    }
}
