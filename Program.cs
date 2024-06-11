// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Saifr.Console;

FabMoMoPaymentRequestModel model = new FabMoMoPaymentRequestModel
{
    CallbackUrl = "https://webhook.site/3122f819-62ec-49f8-9742-f7743bc6ca79",
    Description = "test transactions",
    UserGenCode = null,
    Type = "D",
    MiscNum = "1",
    FirstName = null,
    LastName = null,
    PhoneNumber = "233544797799",
    Amount = 1m,
    Network = "MTN",
    ExtRefNum = "FPMTc0000000000001"
};

Console.WriteLine("Hello, World!");
Console.WriteLine("Original Object: " + JsonConvert.SerializeObject(model, Formatting.Indented));

string encryptedObject = SaifrPro.EncryptObject(model);
Console.WriteLine("Encrypted String: " + encryptedObject);


public class SaifrPro
{
    private static readonly string Key = "346189ca552047f7807f3980c11d9b18"; // 32 bytes key

    public static string Encrypt(string plaintext)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plaintext);
                    }
                }

                byte[] encrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(encrypted);
            }
        }
    }
    
    public static string Decrypt(string encryptedText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(Key);
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] cipherText = Convert.FromBase64String(encryptedText);

            using (var msDecrypt = new MemoryStream(cipherText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    public static string EncryptObject<T>(T obj)
    {
        string jsonString = JsonConvert.SerializeObject(obj);
        return Encrypt(jsonString);
    }

    public static T DecryptObject<T>(string encryptedText)
    {
        string jsonString = Decrypt(encryptedText);
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}