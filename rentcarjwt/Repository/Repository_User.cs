using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using rentcarjwt.Model;
using rentcarjwt.Model.Data;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Services.Kdf;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using rentcarjwt.Services.JwtToken;
using rentcarjwt.Services.Mail;

namespace rentcarjwt.Repository
{
    public class Repository_User : IRepository_User
    {


        private readonly IKdfServise _kdfService;
        private readonly DataContext _context;
        readonly IWebHostEnvironment _appEnvironment;
        private readonly IMailService _mailService;
           private readonly IJwtToken jwtToken;
        public Repository_User(DataContext context, IWebHostEnvironment appEnvironment, IKdfServise kdfService, IMailService mailService, IJwtToken jwtToken)
        {
            _kdfService=kdfService;
            _context = context;
            _appEnvironment = appEnvironment;
            _mailService= mailService;
            this.jwtToken = jwtToken;
        }


        //public async Task<User> GetUser(string id)
        //{
        //    Guid userId = Guid.Parse(id);
        //    return await _context.Users.FindAsync(userId);
        //}

        //public async Task<User> CheckLogin(string login)
        //{
        //    return await  _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        //}

        public string CheckPassword(string password, string sult)
        {
            return _kdfService.GetDerivedKey(password, sult);

        }

        public async Task<User> CheckEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.email == email);
        } 
        public async Task<User> GetUserId(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Id == new Guid(id));
        }
        

        //public async Task<string> UpdateAvatar(IFormFile? uploadedFile, User user)
        //{

        //    if (user.Avatar != null)
        //    {
        //        string fileName = Path.GetFileName(user.Avatar);                                  // Парсим URL - адресу, щоб отримати тільки ім'я файлу (останній сегмент URL-адреси)
        //        string filePath = Path.Combine(_appEnvironment.WebRootPath, "avatar", fileName);  // Отримуємо шлях до файлу на сервері

        //        if (File.Exists(filePath))  // Проверяем, существует ли файл
        //        {
        //            File.Delete(filePath);  // Видаляємо файл
        //        }
        //    }
        //    string Avatar_path=null;
        //    if (uploadedFile != null)
        //        if (uploadedFile.ContentType.StartsWith("image/"))                                               //Перевіряємо чи користувач вибрав картинку
        //        {
        //            string path = "/avatar/" + uploadedFile.FileName;                                            // имя файла                               
        //            Avatar_path = DataContext.WebServerAPI + "/avatar/" + uploadedFile.FileName;
        //            using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
        //            await uploadedFile.CopyToAsync(fileStream);                                                  // копіюємо файл у потік                      
        //            user.Avatar = Avatar_path;                                                                  
        //        }

        //    _context.Update(user);
        //    await _context.SaveChangesAsync();
        //    return Avatar_path;
        //}

       
        public async Task<User> AddUser(RegisterRequest RegisterUser)
        {
            User user = new();

            user.Id = Guid.NewGuid();
            user.salt = Guid.NewGuid().ToString();
            user.firstName = RegisterUser.firstName;
            user.lastName = RegisterUser.lastName;
            user.email = RegisterUser.email;
            user.phone = RegisterUser.phone;
            user.age = RegisterUser.age;
            user.RegsterDt = DateTime.Now;
            user.drivingExperience = user.drivingExperience;
            user.passwordHash = _kdfService.GetDerivedKey(RegisterUser.password, user.salt);
            user.RefreshToken = jwtToken.GenerateRefreshToken();
         
            DateTime currentDateTime = DateTime.Now;
            DateTime refreshTokenExpiryTime = currentDateTime.AddHours(5);
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            Random random = new Random();
            int emailConfirmCode = random.Next(100000, 999999);
            user.emailConfirmCode = emailConfirmCode.ToString();

            await _mailService.SendEmail(user.emailConfirmCode, user.email);

            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUser(User user)
        {
            _context.Update(user);  // Оновлюємо в БД
            await _context.SaveChangesAsync(); 
        }
        public async Task UpdatePassword(string Password, User user)
        {
            user.passwordHash= _kdfService.GetDerivedKey(Password, user.salt);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GeneratePassword(User user)
        {                                                                                           //
            Random random = new Random();                                                              // Створюємо новий пароль із 6 символів
            int randomNumber = random.Next(100000, 999999);                                         //
            user.passwordHash = randomNumber.ToString();                                            //
            user.passwordHash = _kdfService.GetDerivedKey(user.passwordHash, user.salt);   // Хешуємо пароль

            _context.Update(user);                      // Оновлюємо в БД
            await _context.SaveChangesAsync();          // 
            return randomNumber.ToString();             // Повертаємо новий пароль для відправки на пошту користовачу 
        }

    }
}






//public async Task UpdatePassword(string Password, User user)
//{
//    user.PasswordHash = _kdfService.GetDerivedKey(Password, user.Id.ToString());           
//    _context.Update(user);
//    await _context.SaveChangesAsync();
//}

//public async Task<ListMusicAndCountPages> DeleteMyMusic(string _idUser, string _idMusic, int _perPage, int currentPage)
//{
//    Guid idUser = Guid.Parse(_idUser);
//    Guid idMusic = Guid.Parse(_idMusic);
//    try
//    {
//        User us = await _context.Users.FindAsync(idUser);
//        Music mc = await _context.Musics.FirstOrDefaultAsync(m => m.id == idMusic);

//        if (mc != null && us != null)
//        {
//            UserMusic um = await _context.UserMusics.FirstOrDefaultAsync(p => p.UserId == us.Id && p.MusicId == mc.id);
//            _context.UserMusics.Remove(um);
//            await _context.SaveChangesAsync();
//        }

//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//    return await GetMyMusicList(_idUser,  _perPage, currentPage);
//}


