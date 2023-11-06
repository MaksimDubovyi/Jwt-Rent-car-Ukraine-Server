namespace rentcarjwt.Model
{
    public class Filter
    {
        public float? priceFrom { get; set; }                   //  ціна від 
        public float? priceTo { get; set; }                     //  ціна до
        public int? ageFrom { get; set; }                       //  рік від 
        public int? ageTo { get; set; }                         //  рік до
        public DateTime? dateFrom { get; set; }                 //  дата від
        public DateTime? dateTo { get; set; }                   //  дата до
        public bool? climate { get; set; }                      //  кондиціонер
        public string? brand { get; set; } = null!;             //  марка
        public string? model { get; set; }                      //  модель
        public int? door { get; set; }                          //  кількість дверей
        public int? numberOfSeats { get; set; }                 //  кількість місць
        public string? transmissionType { get; set; } = null!;  //  Тип трансмісії
        public string? fuelЕype { get; set; }                   //  тип палива
      
    }
}
