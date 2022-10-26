using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class ProjectInfo
    {
        [JsonProperty("project_number")]
        public string ProjectNumber { get; set; }

        [JsonProperty("firebase_url")]
        public string FirebaseUrl { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("storage_bucket")]
        public string StorageBucket { get; set; }
    }
}