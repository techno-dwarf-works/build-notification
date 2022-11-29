using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class Services
    {
        [JsonConstructor]
        public Services(
            [JsonProperty("appinvite_service")] AppinviteService appinviteService
        )
        {
            AppinviteService = appinviteService;
        }

        [JsonProperty("appinvite_service")]
        public AppinviteService AppinviteService { get; }
    }
}