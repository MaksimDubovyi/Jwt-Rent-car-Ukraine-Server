namespace rentcarjwt.Model.Marker
{
    public class MarkerResponse
    {
        public string Id { get; set; }
        public string IdCar { get; set; }            // якому автомобілю належить цей запис
        public DateTime dateFrom { get; set; }     // вказує на час початку             
        public DateTime? dateTo { get; set; }      // вказує на час завершення
        public double? lat { get; set; }            // широта
        public double lng { get; set; }            // довгота  

        public string UserEmail { get; set; }
        public string? foto { get; set; }
        public float? price { get; set; } = null!;                   //  ціна
        public string brand { get; set; } = null!;                   //  марка
        public string? model { get; set; }                           //  модель
        public int? age { get; set; }                                //  рік
        public string? color { get; set; }                           //  колір

    }
}
