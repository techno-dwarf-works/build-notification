using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class IOSInfo
    {
        [JsonProperty("bundle_id")]
        public string BundleId { get; set; }
    }
}