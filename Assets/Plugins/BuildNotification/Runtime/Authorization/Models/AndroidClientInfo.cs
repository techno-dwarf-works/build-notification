using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
{
    public class AndroidClientInfo
    {
        [JsonConstructor]
        public AndroidClientInfo(
            [JsonProperty("package_name")] string packageName
        )
        {
            PackageName = packageName;
        }

        [JsonProperty("package_name")]
        public string PackageName { get; }
    }
}