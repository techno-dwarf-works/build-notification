using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class AppInviteService
    {
        [JsonProperty("other_platform_oauth_client")]
        public List<OtherPlatformOauthClient> OtherPlatformOauthClient { get; set; }
    }
}