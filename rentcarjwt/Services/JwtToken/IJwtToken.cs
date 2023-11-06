using rentcarjwt.Model.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace rentcarjwt.Services.JwtToken
{
    public interface IJwtToken
    {
        // Створення JWT
        public string GetToken(string email,string firstName);

        //оновлення токену  
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);

        //отримати пошту з токену
        public string getMailFromToken(ClaimsPrincipal principal);

        // Створення GenerateRefreshToken
        public string GenerateRefreshToken();

        // Метод, що створює JWT токен на основі списку клеймів автентифікації 
        public JwtSecurityToken CreateToken(List<Claim> authClaims);

        public string getEmailUser(string accessToken);

    }
}
