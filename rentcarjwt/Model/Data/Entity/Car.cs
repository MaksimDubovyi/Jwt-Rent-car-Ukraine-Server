using rentcarjwt.Model.Car_;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rentcarjwt.Model.Data.Entity
{
    public class Car
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string? foto { get; set; }
        public float? price { get; set; } = null!;                   //  ціна
        public string brand { get; set; } = null!;                   //  марка
        public string? model { get; set; }                           //  модель
        public int? age { get; set; }                                //  рік
        public bool? climate { get; set; }                           //  кондиціонер
        public int? door { get; set; }                               //  кількість дверей
        public string? color { get; set; }                           //  колір
        public int numberOfSeats { get; set; }                       //  кількість місць
        public float engineCapacity { get; set; }                    //  обєм двигуна
        public string transmissionType { get; set; } = null!;        //  Тип трансмісії
        public string? fuelEype { get; set; }                        //  тип палива
        public StatusCar? status { get; set; }                       //  тип палива

        public DateTime dateСreation { get; set; }                   //  дата створення
        public DateTime? deleteDt { get; set; }
    }

 
}


