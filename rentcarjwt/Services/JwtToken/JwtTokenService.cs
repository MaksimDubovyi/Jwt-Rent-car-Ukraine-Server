using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using rentcarjwt.Model.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace rentcarjwt.Services.JwtToken
{
    public class JwtTokenService: IJwtToken
    {
        private readonly IConfiguration _configuration;
        String Secret;
        String Issuer;
        String Audience;
        int Expire;


        public JwtTokenService(IConfiguration configuration)
        {
            _configuration= configuration;
            var gmailConfig = _configuration.GetSection("Jwt");          // Отримання конфігурації JWT з файлу налаштувань
            this.Secret = gmailConfig.GetValue<String>("Secret")!;       // Отримання секретного ключа для підпису JWT
            this.Issuer = gmailConfig.GetValue<String>("Issuer")!;       // Отримання власника JWT
            this.Audience = gmailConfig.GetValue<String>("Audience")!;   // Отримання аудиторії JWT
            this.Expire = gmailConfig.GetValue<int>("Expire");           // Отримання часу життя JWT у хвилинах
        }
     
        // Створення JWT
        public string GetToken(string email, string firstName)
        {
            List<Claim> claims = new List<Claim>();           // Створення списку клеймів для JWT

            claims.Add(new Claim("Email", email));            // Додавання клейму "Email" з переданим значенням електронної пошти
            claims.Add(new Claim("FirstName", firstName));    // Додавання довільного клейму "level" зі значенням "123"
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));  // Додавання ролі "Admin" як клейму

            // Створення симетричного ключа для підпису JWT на основі секретного ключа
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret));

            var token = new JwtSecurityToken( // Створення JWT з відповідними параметрами
               issuer: this.Issuer,
               audience: this.Audience,
               claims: claims,
               expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(this.Expire)),
               notBefore: DateTime.UtcNow,
               signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
           );

            // Повернення JWT у вигляді рядка
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //оновлення токену та   пееревірка валідності підпису токену 
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            // Налаштування параметрів перевірки токена
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,            // Вимкнення перевірки аудиторії
                ValidateIssuer = false,              // Вимкнення перевірки власника
                ValidateIssuerSigningKey = true,     // Включення перевірки підпису власного ключа
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),  // Встановлення симетричного ключа для перевірки підпису
                ValidateLifetime = false             // Вимкнення перевірки часу життя токена
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Перевірка токена з використанням встановлених параметрів
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            // Перевірка, чи токен використовує алгоритм підпису HmacSha256
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            // Повертаємо представлення клеймів користувача
            return principal;
        }

        //отримати пошту з токену
        public string getMailFromToken(ClaimsPrincipal principal)
        {
            string email = "";
            var claims = principal.Claims;
            // Для кожного клейму вивести ключ та значення
            foreach (var claim in claims)
            {
                if (claim.Type == "Email")
                    email = claim.Value;
            }
            return email;
        }

        // Створення GenerateRefreshToken
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // Метод, що створює JWT токен на основі списку клеймів автентифікації 
        public JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            // Створення симетричного ключа для підпису JWT на основі секретного ключа
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret));

            var token = new JwtSecurityToken(// Створення JWT з відповідними параметрами
                issuer: this.Issuer,
                audience: this.Audience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(this.Expire)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return token;  // Повертаємо створений JWT токен.
        }

        public string getEmailUser(string accessToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            return getMailFromToken(principal);
        }

    }
}
