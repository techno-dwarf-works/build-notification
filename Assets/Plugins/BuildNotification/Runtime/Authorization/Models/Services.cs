using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
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