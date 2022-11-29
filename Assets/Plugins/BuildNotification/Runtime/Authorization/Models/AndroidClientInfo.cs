using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
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