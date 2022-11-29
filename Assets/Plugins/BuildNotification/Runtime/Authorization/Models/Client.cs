using System.Collections.Generic;
using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class Client
    {
        [JsonConstructor]
        public Client(
            [JsonProperty("client_info")] ClientInfo clientInfo,
            [JsonProperty("oauth_client")] List<OauthClient> oauthClient,
            [JsonProperty("api_key")] List<ApiKey> apiKey,
            [JsonProperty("services")] Services services
        )
        {
            ClientInfo = clientInfo;
            OauthClient = oauthClient;
            ApiKey = apiKey;
            Services = services;
        }

        [JsonProperty("client_info")]
        public ClientInfo ClientInfo { get; }

        [JsonProperty("oauth_client")]
        public IReadOnlyList<OauthClient> OauthClient { get; }

        [JsonProperty("api_key")]
        public IReadOnlyList<ApiKey> ApiKey { get; }

        [JsonProperty("services")]
        public Services Services { get; }
    }
}