using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Services.Mail
{
    public interface IMailService
    {
        Task SendEmail(string user_key, string user_mail);
        Task SendEmailNewPassword(string password, string user_mail);
        Task SendEmailConfirmationRent(Car car, string user_mail, string tenantEmail);
        Task SendEmailCancelRent(Car car, string user_mail, string tenantEmail);
    }
}
