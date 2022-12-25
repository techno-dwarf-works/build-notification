using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
{
    public class ProjectInfo
    {
        [JsonConstructor]
        public ProjectInfo(
            [JsonProperty("project_number")] string projectNumber,
            [JsonProperty("firebase_url")] string firebaseUrl,
            [JsonProperty("project_id")] string projectId,
            [JsonProperty("storage_bucket")] string storageBucket
        )
        {
            ProjectNumber = projectNumber;
            FirebaseUrl = firebaseUrl;
            ProjectId = projectId;
            StorageBucket = storageBucket;
        }

        [JsonProperty("project_number")]
        public string ProjectNumber { get; }

        [JsonProperty("firebase_url")]
        public string FirebaseUrl { get; }

        [JsonProperty("project_id")]
        public string ProjectId { get; }

        [JsonProperty("storage_bucket")]
        public string StorageBucket { get; }
    }
}