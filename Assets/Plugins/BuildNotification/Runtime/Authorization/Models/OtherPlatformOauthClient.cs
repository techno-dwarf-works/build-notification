using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
{
    public class OtherPlatformOauthClient
    {
        [JsonConstructor]
        public OtherPlatformOauthClient(
            [JsonProperty("client_id")] string clientId,
            [JsonProperty("client_type")] int clientType,
            [JsonProperty("ios_info")] IosInfo iosInfo
        )
        {
            ClientId = clientId;
            ClientType = clientType;
            IosInfo = iosInfo;
        }

        [JsonProperty("client_id")]
        public string ClientId { get; }

        [JsonProperty("client_type")]
        public int ClientType { get; }

        [JsonProperty("ios_info")]
        public IosInfo IosInfo { get; }
    }
}