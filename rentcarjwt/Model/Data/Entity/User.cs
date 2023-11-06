using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations;

namespace rentcarjwt.Model.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string? firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string? email { get; set; }
        public string? emailConfirmCode { get; set; }
        public string? phone { get; set; }
        public string? age { get; set; }
        public string? drivingExperience { get; set; }
        public string? avatar { get; set; }
        public string passwordHash { get; set; } = null!;
        public string salt { get; set; } = null!;
        public DateTime RegsterDt { get; set; }
        public DateTime? DeleteDt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}




