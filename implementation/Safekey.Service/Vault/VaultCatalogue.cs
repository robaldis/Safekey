
namespace Safekey.Service.Vault;

public class VaultCatalogue : IVaultCatalogue
{
    private readonly IVaultRegistry _registry;

    public VaultCatalogue(
        IVaultRegistry vaultRegistry)
    {
        _registry = vaultRegistry;
    }

    public string CreateSecret(string key, string secret)
    {
        var (hash, iv) = Encryption.Encryption.Encrypt(secret);
        _registry.CreateSecret(key, hash, iv);
        return key;
    }


    public string GetSecret(string key)
    {
        var secret = _registry.GetSecret(key);
        var plainText = Encryption.Encryption.Decrypt(secret.Value, secret.IV);
        return plainText;
    }

    public void DeleteSecret(string key)
    {
        _registry.DeleteSecret(key);
    }
}



