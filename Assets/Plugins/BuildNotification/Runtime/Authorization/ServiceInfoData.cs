using System.Collections.Generic;
using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class ServiceInfoData
    {
        [JsonConstructor]
        public ServiceInfoData(
            [JsonProperty("project_info")] ProjectInfo projectInfo,
            [JsonProperty("client")] List<Client> client,
            [JsonProperty("configuration_version")] string configurationVersion
        )
        {
            ProjectInfo = projectInfo;
            Client = client;
            ConfigurationVersion = configurationVersion;
        }

        [JsonProperty("project_info")]
        public ProjectInfo ProjectInfo { get; }

        [JsonProperty("client")]
        public IReadOnlyList<Client> Client { get; }

        [JsonProperty("configuration_version")]
        public string ConfigurationVersion { get; }
    }
}