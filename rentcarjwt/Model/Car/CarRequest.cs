using Microsoft.AspNetCore.Mvc;

namespace rentcarjwt.Model.Car_
{
    public class CarRequest
    {
        
        public string AccessToken { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public bool Climate { get; set; }
        public int Door { get; set; } = 0;
        public string Color { get; set; } 
        public int Price { get; set; }
        public int Age { get; set; }
        public int numberOfSeats { get; set; }
        public int engineCapacity { get; set; }
        public string transmissionType { get; set; }
        public string fuelЕype { get; set; }
        public string Foto { get; set; }
    }
}
