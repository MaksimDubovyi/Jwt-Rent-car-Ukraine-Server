namespace rentcarjwt.Model.Data.Entity
{
    public class MarkerCar
    {
        public Guid Id { get; set; }
        public Guid IdCar { get; set; }            // якому автомобілю належить цей запис
        public double lat { get; set; }            // широта
        public double lng { get; set; }            // довгота                             
        public DateTime fromDate { get; set; }     // вказує на час початку             
        public DateTime? toDate { get; set; }      // вказує на час завершення
    }
}
