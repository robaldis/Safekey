using System.Text;

namespace Safekey.Service.Vault;

public class VaultCatalogue()
{

    public (byte[], byte[]) CreateSecret(string secret)
    {
        var bytes = Encryption.Encryption.Encrypt(secret);
        return bytes;
    }


    public string GetSecret(byte[] secret, byte[] IV)
    {
        var plainText = Encryption.Encryption.Decrypt(secret, IV);
        return plainText;
    }
}



