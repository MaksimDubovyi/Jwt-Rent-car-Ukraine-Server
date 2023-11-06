namespace rentcarjwt.Model
{
    public class AuthResponse
    {
        public string firstName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string accessToken { get; set; } = null!;
        public string refreshToken { get; set; } = null!;
        public string emailConfirmCode { get; set; } = null!;
        public string avatar { get; set; } = null!;
    }
    
}
