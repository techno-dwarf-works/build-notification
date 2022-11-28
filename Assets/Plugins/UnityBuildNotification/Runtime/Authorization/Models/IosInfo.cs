using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
{
    public class IosInfo
    {
        [JsonConstructor]
        public IosInfo(
            [JsonProperty("bundle_id")] string bundleId
        )
        {
            BundleId = bundleId;
        }

        [JsonProperty("bundle_id")]
        public string BundleId { get; }
    }
}