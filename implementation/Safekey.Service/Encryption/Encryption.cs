
using System.Security.Cryptography;
using System.Text;

namespace Safekey.Service.Encryption;

public static class Encryption
{

    //public static string Encryption()
    //{
    //    var expected = "something";
    //    byte[] enc;
    //    using (Aes myAes = Aes.Create())
    //    {
    //        //Aes.Key = Convert.ToBase64String("secret");
    //        Console.WriteLine(System.Text.Encoding.UTF8.GetString(myAes.Key));
    //        // Initialisation Vector
    //        Console.WriteLine(System.Text.Encoding.UTF8.GetString(myAes.IV));
    //        enc = EncryptStringToBytes_Aes(expected, myAes.Key, myAes.IV);
    //        var result = DecryptStringFromBytes_Aes(enc, myAes.Key, myAes.IV);
    //        result = DecryptStringFromBytes_Aes(enc, myAes.Key, myAes.IV);
    //        Console.WriteLine($"Expected: {expected}, Encrypted: {System.Text.Encoding.UTF8.GetString(enc)}, Result: {result}");
    //    }

    //    return expected;

    //}

    public static (byte[], byte[]) Encrypt(string secret)
    {
        using (Aes myAes = Aes.Create())
        {
            myAes.Key = GetAesKey();
            return (EncryptStringToBytes_Aes(secret, myAes.Key, myAes.IV), myAes.IV);
        }
    }

    public static string Decrypt(byte[] secret, byte[] IV)
    {
        var key = GetAesKey();
        return DecryptStringFromBytes_Aes(secret, key, IV);
    }

    private static byte[] GetAesKey()
    {
        //string? base64Key = Environment.GetEnvironmentVariable("AES_KEY");
        string base64Key = "HN9jaSFJ5IS/+mN6bbeGfGe8rTRswJqBx+9fccSGNQg=";

        if (string.IsNullOrEmpty(base64Key))
            throw new InvalidOperationException("AES_KEY environment variable is not set.");

        var key = Convert.FromBase64String(base64Key);
        if (key.Length != 32)
            throw new InvalidOperationException($"AES_KEY must be 32 bytes (256 bits) in base64 format. {key.Length}");
        return key;
    }

    private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }

                encrypted = msEncrypt.ToArray();
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }

    private static byte[] CreateAesKey(string inputString)
    {
        return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
}
