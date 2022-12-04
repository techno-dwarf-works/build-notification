using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Better.BuildNotification.Platform.Tooling;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Services
{
    public static class FileLoadService
    {
        public static async Task<T> LoadFileAsync<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
#if UNITY_2021_3_OR_NEWER
                var str = await File.ReadAllTextAsync(path);
#else
                var str = File.ReadAllText(path);
#endif
                return await Task.Run(() =>
                {
                    var fromBase64String = Convert.FromBase64String(str);
                    var value = Encoding.UTF8.GetString(fromBase64String);
                    return JsonConvert.DeserializeObject<T>(value);
                });
            }

            return default;
        }

        public static T LoadFile<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                var str = File.ReadAllText(path);
                var fromBase64String = Convert.FromBase64String(str);
                var value = Encoding.UTF8.GetString(fromBase64String);
                return JsonConvert.DeserializeObject<T>(value);
            }

            return null;
        }
        
        public static T LoadEncryptedFile<T>(string path, byte[] key) where T : class
        {
            if (File.Exists(path))
            {
                var str = File.ReadAllText(path);
                var fromBase64String = Convert.FromBase64String(str);
                var decrypted =
                    Encryption.Decrypt(fromBase64String, key);
                if (decrypted == null) return null;
                var value = Encoding.UTF8.GetString(decrypted);
                return JsonConvert.DeserializeObject<T>(value);
            }

            return null;
        }
        
        public static void SaveEncryptedFile<T>(string path, T data, byte[] key) where T : class
        {
            var serializeObject = JsonConvert.SerializeObject(data, Formatting.Indented);
            var bytes = Encoding.UTF8.GetBytes(serializeObject);
            var encrypted =
                Encryption.Encrypt(bytes, key);
            var base64 = Convert.ToBase64String(encrypted);
            File.WriteAllText(path, base64);
        }

        public static async Task<T> LoadJsonFileAsync<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
#if UNITY_2021_3_OR_NEWER
                var str = await File.ReadAllTextAsync(path);
#else
                var str = await Task.Run(() => File.ReadAllText(path));
#endif
                return JsonConvert.DeserializeObject<T>(str);
            }

            return default;
        }

        public static T LoadJsonFile<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                var str = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(str);
            }

            return default;
        }

        public static void DeleteFile(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
        }
        
        public static void SaveFile<T>(string path, T data) where T : class
        {
            var serializeObject = JsonConvert.SerializeObject(data, Formatting.Indented);
            var bytes = Encoding.UTF8.GetBytes(serializeObject);
            var base64 = Convert.ToBase64String(bytes);
            File.WriteAllText(path, base64);
        }

        public static async Task SaveFileAsync<T>(string path, T data) where T : class
        {
            var serialized = await Task.Run(() =>
            {
                var serializeObject = JsonConvert.SerializeObject(data, Formatting.Indented);
                var bytes = Encoding.UTF8.GetBytes(serializeObject);
                return Convert.ToBase64String(bytes);
            });
#if UNITY_2021_3_OR_NEWER
            await File.WriteAllTextAsync(path, serialized);
#else
            await Task.Run(() => File.WriteAllText(path, serialized));
#endif
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}