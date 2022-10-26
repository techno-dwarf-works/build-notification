using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class Client
    {
        [JsonProperty("client_info")]
        public ClientInfo ClientInfo { get; set; }

        [JsonProperty("oauth_client")]
        public List<OauthClient> OauthClient { get; set; }

        [JsonProperty("api_key")]
        public List<ApiKey> ApiKey { get; set; }

        [JsonProperty("services")]
        public Services Services { get; set; }
    }
}