
namespace Safekey.Service.Vault;

public interface IVaultCatalogue
{


    public string CreateSecret(string key, string secret);

    public string GetSecret(string key);

    public void DeleteSecret(string key);
}
