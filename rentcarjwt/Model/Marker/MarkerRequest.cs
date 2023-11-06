using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rentcarjwt.Model.Marker
{
    public class MarkerRequest
    {
        public string accessToken { get; set; }
        public string idCar { get; set; }            // якому автомобілю належить цей запис
        public DateTime dateFrom { get; set; }     // вказує на час початку             
        public DateTime dateTo { get; set; }      // вказує на час завершення
        public double lat { get; set; }            // широта
        public double lng { get; set; }            // довгота                             

    }
}
//accessToken: string;
//idCar: string,               //  id авто
//dateFrom:Date,            //  дата з
//  dateTo: Date,              //  дата до
//  lat: number                  //  Широта (latitude)
//  lng:number                  //  довгота (longitude)
