using rentcarjwt.Repository;
using rentcarjwt.Services.Hash;
using rentcarjwt.Services.JwtToken;
using rentcarjwt.Services.Kdf;
using rentcarjwt.Services.Mail;

namespace servercar.Repository
{
    public static class ServiceProviderExtensions
    {
        public static void AddMyService(this IServiceCollection services)
        {
            services.AddScoped<IRepository_User, Repository_User>();
            services.AddScoped<IHashservice, Md5Hashservice>();
            services.AddScoped<IHashservice, Sh1Hashservice>();
            services.AddScoped<IKdfServise, HashBasedKdfService>();
            services.AddScoped<IJwtToken, JwtTokenService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IRepository_Car, Repository_Car>();
            services.AddScoped<IRepository_MarkerCar, Repository_MarkerCar>();
            services.AddScoped<IRepository_Reserve, Repository_Reserve>();
            services.AddScoped<IRepository_Message, Repository_Message>();

        }
        


    }
}
