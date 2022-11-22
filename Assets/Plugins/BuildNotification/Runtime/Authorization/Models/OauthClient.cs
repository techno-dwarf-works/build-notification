using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
{
    public class OauthClient
    {
        [JsonConstructor]
        public OauthClient(
            [JsonProperty("client_id")] string clientId,
            [JsonProperty("client_type")] int clientType
        )
        {
            ClientId = clientId;
            ClientType = clientType;
        }

        [JsonProperty("client_id")]
        public string ClientId { get; }

        [JsonProperty("client_type")]
        public int ClientType { get; }
    }
}