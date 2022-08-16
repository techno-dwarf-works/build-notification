using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BetterExtensions.Runtime.Extension;

namespace BuildNotification.Runtime
{
    public static class FileLoader
    {
        public static async Task<T> LoadSecuredFile<T>(string path, string salt) where T : class
        {
            if (File.Exists(path))
            {
#if UNITY_2021_3_OR_NEWER
                var str = await File.ReadAllTextAsync(path);
#else
                var str = File.ReadAllText(path);
#endif
                var decrypted = await EncryptService.DecryptStringToBytes(str, salt);
                return await decrypted.DeserializeAsync<T>();
            }

            return default;
        }


        public static async void SaveSecuredFile<T>(string path, T data, string salt) where T : class
        {
            var serialized = await data.SerializeAsync();
            var secured = await EncryptService.EncryptString(serialized, salt);

#if UNITY_2021_3_OR_NEWER
            await File.WriteAllTextAsync(path, secured);
#else
            File.WriteAllText(path, secured);
#endif
        }

        public static async Task<T> LoadFile<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
#if UNITY_2021_3_OR_NEWER
                var str = await File.ReadAllTextAsync(path);
#else
                var str = File.ReadAllText(path);
#endif
                return await Convert.FromBase64String(str).DeserializeAsync<T>();
            }

            return default;
        }

        public static async Task<T> LoadJsonFile<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
#if UNITY_2021_3_OR_NEWER
                var str = await File.ReadAllTextAsync(path);
#else
                var str = File.ReadAllText(path);
#endif
                return JsonConvert.DeserializeObject<T>(str);
            }

            return default;
        }


        public static async Task SaveFile<T>(string path, T data) where T : class
        {
            var serialized = await data.SerializeAsync();
#if UNITY_2021_3_OR_NEWER
            await File.WriteAllTextAsync(path, Convert.ToBase64String(serialized));
#else
            File.WriteAllText(path, Convert.ToBase64String(serialized));
#endif
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}