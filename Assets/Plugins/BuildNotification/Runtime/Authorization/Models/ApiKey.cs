using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.Models
{
    public class ApiKey
    {
        [JsonConstructor]
        public ApiKey(
            [JsonProperty("current_key")] string currentKey
        )
        {
            CurrentKey = currentKey;
        }

        [JsonProperty("current_key")]
        public string CurrentKey { get; }
    }
}