

using Microsoft.AspNetCore.Mvc;
using rentcarjwt.Model;
using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Repository
{
    public interface IRepository_User
    {
    //    Task<User> GetUser(string id);
        //Task<User> CheckLogin(string login);
        string CheckPassword(string password, string sult);
        Task<User> CheckEmail(string email);
        Task<User> GetUserId(string id);
        Task UpdatePassword(string Password, User user);
        Task<User> AddUser(RegisterRequest user);
        Task UpdateUser(User user);
        // Task<string> UpdateAvatar(IFormFile? uploadedFile,User user);
        Task<string> GeneratePassword(User user);
       //Task UpdatePassword(string Password, User user);
       // Task AddMyMusic(string idUser,  string idMusic);
       //Task<ListMusicAndCountPages> DeleteMyMusic(string idUser, string idMusic, int _perPage, int currentPage);
       //Task<ListMusicAndCountPages> GetMyMusicList(string id, int _perPage, int currentPage);

    }
    

}

