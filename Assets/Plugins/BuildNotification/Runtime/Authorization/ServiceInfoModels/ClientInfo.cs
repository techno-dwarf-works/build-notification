using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class ClientInfo
    {
        [JsonProperty("mobilesdk_app_id")]
        public string MobilesdkAppId { get; set; }

        [JsonProperty("android_client_info")]
        public AndroidClientInfo AndroidClientInfo { get; set; }
    }
}