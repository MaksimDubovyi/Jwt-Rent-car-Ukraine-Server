namespace rentcarjwt.Model.Data.Entity
{
    public class ReserveCar
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; } // Зовнішній ключ на таблицю User
        public User User { get; set; }

        public Guid CarId { get; set; } // Зовнішній ключ на таблицю Car
        public Car Car { get; set; }
    }
}