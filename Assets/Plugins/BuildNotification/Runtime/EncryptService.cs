using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildNotification.Runtime
{
    public static class EncryptService
    {
        public static async Task<string> EncryptString(byte[] input, string key)
        {
            var base64 = Convert.ToBase64String(input);
            return await EncryptString(base64, key);
        }

        public static async Task<string> EncryptString(string input, string key)
        {
            var iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
#if UNITY_2021_3_OR_NEWER
                await
#endif
                    using (var memoryStream = new MemoryStream())
                {
#if UNITY_2021_3_OR_NEWER
                    await
#endif
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
#if UNITY_2021_3_OR_NEWER
                        await
#endif
                            using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            await streamWriter.WriteAsync(input);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static async Task<byte[]> DecryptStringToBytes(string input, string key)
        {
            var decrypted = await DecryptString(input, key);
            return Convert.FromBase64String(decrypted);
        }

        public static async Task<string> DecryptString(string input, string key)
        {
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(input);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

#if UNITY_2021_3_OR_NEWER
                await
#endif
                    using (var memoryStream = new MemoryStream(buffer))
                {
#if UNITY_2021_3_OR_NEWER
                    await
#endif
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }
        }
    }
}