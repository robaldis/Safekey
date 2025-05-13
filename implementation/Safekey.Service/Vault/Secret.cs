
namespace Safekey.Service.Vault;

public class Secret
{
    public string Key { get; }
    public byte[] Value { get; }
    public byte[] IV { get; }

    public Secret(
        string key,
        byte[] value,
        byte[] iv)
    {
        Key = key;
        Value = value;
        IV = iv;
    }
}
