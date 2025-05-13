
namespace Safekey.Service.Vault;

public interface IVaultRegistry
{
    public void CreateSecret(string key, byte[] secret, byte[] iv);

    public Secret GetSecret(string key);

    public void DeleteSecret(string key);
}
