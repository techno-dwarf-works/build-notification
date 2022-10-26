using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class ApiKey
    {
        [JsonProperty("current_key")]
        public string CurrentKey { get; set; }
    }
}