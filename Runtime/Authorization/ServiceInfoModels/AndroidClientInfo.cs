using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class AndroidClientInfo
    {
        [JsonProperty("package_name")]
        public string PackageName { get; set; }
    }
}