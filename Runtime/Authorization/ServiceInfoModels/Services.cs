using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class Services
    {
        [JsonProperty("appinvite_service")]
        public AppInviteService AppInviteService { get; set; }
    }
}