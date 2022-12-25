using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
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