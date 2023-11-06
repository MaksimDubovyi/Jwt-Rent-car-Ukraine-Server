
using rentcarjwt.Model.Car_;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rentcarjwt.Model.Car
{
    public class TenantCars
    {
        public Guid IdCar { get; set; }
        public string UserEmail { get; set; }
        public string? foto { get; set; }
        public float? price { get; set; } = null!;                 
        public string brand { get; set; } = null!;                 
        public string? model { get; set; }                         
        public StatusCar? status { get; set; }                     

    }
}
