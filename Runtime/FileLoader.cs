using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BetterExtensions.Runtime.Extension;

namespace BuildNotification.Runtime
{
    public static class FileLoader
    {
        public static async Task<T> LoadFile<T>(string path) where T : class
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

        public static void DeleteFile(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
        }

        public static async Task SaveFile<T>(string path, T data) where T : class
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
            File.WriteAllText(path, Convert.ToBase64String(serialized));
#endif
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}