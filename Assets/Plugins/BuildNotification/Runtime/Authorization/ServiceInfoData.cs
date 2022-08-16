using System;
using System.Collections.Generic;
using BuildNotification.Runtime.Authorization.ServiceInfoModels;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization
{
    [Serializable]
    public class ServiceInfoData
    {
        [JsonProperty("project_info")]
        public ProjectInfo ProjectInfo { get; set; }

        [JsonProperty("client")]
        public List<Client> Client { get; set; }

        [JsonProperty("configuration_version")]
        public string ConfigurationVersion { get; set; }
    }
}