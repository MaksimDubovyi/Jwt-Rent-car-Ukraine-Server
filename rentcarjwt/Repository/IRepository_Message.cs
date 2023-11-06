using Microsoft.AspNetCore.Mvc;
using rentcarjwt.Model.Marker;
using rentcarjwt.Model.MessageModel;

namespace rentcarjwt.Repository
{
    public interface IRepository_Message
    {
        Task<MessageResponse> sendMessage(string emailSender, string idCar, string txt);
        Task<MessageResponse> sendMessage2(string idCar, string token, string txt, string? userLessorId, string? userTenantId);
        Task <List<MessageResponse>> getAllMessageIdCar(string emailSender, string idCar);
        Task <List<MessageResponse>> getAllMessageIdCarTL(string IdUser1, string IdUser2, string idCar, string emailSender);
        Task<int> getCountNoRaedMessage(string myEmail);
        Task<List<MessageList>>  getLatestMessages(string myEmail);
    }
}
