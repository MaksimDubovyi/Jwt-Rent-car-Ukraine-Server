using rentcarjwt.Services.Hash;

namespace rentcarjwt.Services.Kdf
{
    public class HashBasedKdfService : IKdfServise
    {
        private readonly IHashservice _hashservice;

        public HashBasedKdfService(IHashservice hashservice)
        {
            _hashservice = hashservice;
        }
    
        public string GetDerivedKey(string password, string salt)
        {
            return _hashservice.GetHash(password+ salt);
        }
    }
}
