using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Authorization
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