namespace rentcarjwt.Services.Hash
{
    public class Md5Hashservice : IHashservice
    {
        public string GetHash(string text)
        {
            using var hasher=System.Security.Cryptography.MD5.Create();

           return Convert.ToHexString(
            hasher.ComputeHash(
            System.Text.Encoding.UTF8.GetBytes(text)));
        }
    }
}
