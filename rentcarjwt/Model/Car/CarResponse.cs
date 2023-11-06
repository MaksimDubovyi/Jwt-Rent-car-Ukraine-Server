namespace rentcarjwt.Model.Car_
{
    public class CarResponse
    {
        public Guid id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public bool? Climate { get; set; }
        public int? Door { get; set; } = 0;
        public string Color { get; set; }
        public float? Price { get; set; }
        public int? Age { get; set; }
        public int numberOfSeats { get; set; }
        public float engineCapacity { get; set; }
        public string transmissionType { get; set; }
        public string fuelЕype { get; set; }
        public DateTime DateCreation { get; set; }
        public string Foto { get; set; }
        public StatusCar? status { get; set; }
    }
}
