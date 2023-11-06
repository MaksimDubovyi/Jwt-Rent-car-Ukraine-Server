using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Services.Mail
{
    public class MailService: IMailService
    {
        String key;
        String host;
        String box;
        int port;
        bool ssl;
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            var gmailConfig = _configuration.GetSection("Gmail");
              key = gmailConfig.GetValue<String>("Key")!;
              host = gmailConfig.GetValue<String>("Host")!;
              box = gmailConfig.GetValue<String>("Box")!;
              port = gmailConfig.GetValue<int>("Port");
              ssl = gmailConfig.GetValue<bool>("Ssl");
        }


        //Для підтвердження пошти
        public async Task SendEmail(string user_key, string user_mail)
        {
            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)

            };
            try
            {

                //быльш складний варыант 
                MailMessage mailMessage = new()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Rent Car Ua",
                    Body = $"<h2 style='color:red'>RentCarUa</h2>" +
                    $"<p>Для підтвердження пошти використайте код <i style='color:green'>{user_key}</i></p>" +
                    $"Вітаємо з реєстрацією на <a href='https://rentcarua.azurewebsites.net/' >RentCarUa</a>"
                };
                mailMessage.To.Add(new MailAddress(user_mail));


              await  smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex) { String message = $"Помилка {ex.Message}"; }
        }

        //Для оновлення паролю 
        public async Task SendEmailNewPassword(string password, string user_mail)
        {
            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)

            };
            try
            {

                //більш складний варіант 
                MailMessage mailMessage = new()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Rent Car Ua",
                    Body = $"<h2 style='color:red'>RentCarUa</h2>" +
                    $"<p>Ваш новий пароль <i style='color:green'>{password}</i></p>" +
                    $"Рекомендуємо його змінити на новий в особистому кабінеті <a href='https://rentcarua.azurewebsites.net/' >RentCarUa</a>"
                };
                mailMessage.To.Add(new MailAddress(user_mail));


                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (Exception ex) { String message = $"Помилка {ex.Message}"; }
        }

        //Для підтвердження оренди
        public async Task SendEmailConfirmationRent(Car car, string user_mail, string tenantEmail)
        {
            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)

            };
            try
            {

                //більш складний варіант 
                MailMessage mailMessage = new()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Rent Car Ua",
                    Body = $"<h2 style='color:red'>RentCarUa</h2>" +
                    $"<p>Користувач {tenantEmail} хоче зняти авто в оренду </p>" +
                    $"<p> <i style = 'color:green'>{car.brand} { car.model }</i> {car.price} грн</p>" +
                    $"Для підтвердження перейдіть в особистий кабінет <a href='https://rentcarua.azurewebsites.net/'>RentCarUa</a>"
                };
                mailMessage.To.Add(new MailAddress(user_mail));


                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex) { String message = $"Помилка {ex.Message}"; }
        }  
        
        //Для підтвердження оренди
        public async Task SendEmailCancelRent(Car car, string user_mail, string tenantEmail)
        {
            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)

            };
            try
            {

                //більш складний варіант 
                MailMessage mailMessage = new()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Rent Car Ua",
                    Body =
                    $"<div style='margin:10%;'>"+
                    $"<h2 style='color:red'>RentCarUa</h2>" +
                    $"<p>Користувач {tenantEmail} відмінив оренду </p>" +
                    $"<p style = 'color:green'>{ car.model } <i> {car.brand} </i></p>" +
                    $"<p>{car.price} грн</p>" +
                   $"</div>"
                };
                mailMessage.To.Add(new MailAddress(user_mail));


                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex) { String message = $"Помилка {ex.Message}"; }
        }
    }
}
