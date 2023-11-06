using rentcarjwt.Model;
using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Marker;

namespace rentcarjwt.Repository
{
    public interface IRepository_MarkerCar
    {
        Task<MarkerResponse> Create(MarkerRequest markerRequest, string usermail);
        Task<List<MarkerResponse>> FiltertMarkerList(Filter filter);
        Task<List<MarkerResponse>> GetMarkerList();
        Task RemoveFromMap(string idCar);
    }
}
