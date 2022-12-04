using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class ClientInfo
    {
        [JsonConstructor]
        public ClientInfo(
            [JsonProperty("mobilesdk_app_id")] string mobilesdkAppId,
            [JsonProperty("android_client_info")] AndroidClientInfo androidClientInfo
        )
        {
            MobilesdkAppId = mobilesdkAppId;
            AndroidClientInfo = androidClientInfo;
        }

        [JsonProperty("mobilesdk_app_id")]
        public string MobilesdkAppId { get; }

        [JsonProperty("android_client_info")]
        public AndroidClientInfo AndroidClientInfo { get; }
    }
}