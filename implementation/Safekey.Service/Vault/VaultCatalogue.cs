
namespace Safekey.Service.Vault;

public class VaultCatalogue : IVaultCatalogue
{

    public string CreateSecret(string key, string secret)
    {
        //var bytes = Encryption.Encryption.Encrypt(secret);
        //return bytes;
        return "";
    }


    public string GetSecret(string key)
    {
        //var plainText = Encryption.Encryption.Decrypt(secret, IV);
        //return plainText;
        return "";
    }

    public void DeleteSecret(string key)
    {
        //var plainText = Encryption.Encryption.Decrypt(secret, IV);
        //return plainText;
    }
}



