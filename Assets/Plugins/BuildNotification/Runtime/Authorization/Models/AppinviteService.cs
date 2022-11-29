using System.Collections.Generic;
using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class AppinviteService
    {
        [JsonConstructor]
        public AppinviteService(
            [JsonProperty("other_platform_oauth_client")] List<OtherPlatformOauthClient> otherPlatformOauthClient
        )
        {
            OtherPlatformOauthClient = otherPlatformOauthClient;
        }

        [JsonProperty("other_platform_oauth_client")]
        public IReadOnlyList<OtherPlatformOauthClient> OtherPlatformOauthClient { get; }
    }
}