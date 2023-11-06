namespace rentcarjwt.Services.Hash
{
    public class Sh1Hashservice : IHashservice
    {
        public string GetHash(string text)
        {
            using var hasher = System.Security.Cryptography.SHA1.Create();

            return Convert.ToHexString(
             hasher.ComputeHash(
             System.Text.Encoding.UTF8.GetBytes(text)));
        }
    }
}
