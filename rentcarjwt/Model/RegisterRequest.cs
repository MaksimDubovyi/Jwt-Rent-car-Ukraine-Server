using System.ComponentModel.DataAnnotations;

namespace rentcarjwt.Model
{
    public class RegisterRequest
    {
        [Required]
        public string? firstName { get; set; } = null!;
        [Required]
        public string lastName { get; set; } = null!;
        [Required]
        public string? email { get; set; }
        [Required]
        public string? phone { get; set; }
        [Required]
        public string? age { get; set; }
        [Required]
        public string? drivingExperience { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; } = null!;

        public string confirmPassword { get; set; } = null!;
    }
}
