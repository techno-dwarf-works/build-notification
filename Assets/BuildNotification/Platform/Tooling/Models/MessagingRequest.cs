using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    public class MessagingRequest
    {
        public MessagingRequest(MessagingRequestBody message)
        {
            Message = message;
        }
        
        [JsonProperty("message")] public MessagingRequestBody Message { get; }
    }
}