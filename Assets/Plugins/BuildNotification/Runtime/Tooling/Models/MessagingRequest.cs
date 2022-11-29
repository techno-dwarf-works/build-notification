using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.Tooling.Models
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