namespace rentcarjwt.Services.Kdf
{
    //
    //Key Derivation Function Service (by RFC-2898  https://datatracker.ietf.org/doc/html/rfc2898)
    //
    public interface IKdfServise
    {
        String GetDerivedKey(String password, String salt);
    }
}
