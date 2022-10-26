using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.Models
{
    public class Notification
    {
        public Notification(string title, string body)
        {
            Title = title;
            Body = body;
        }

        [JsonProperty("title")]public string Title { get; set; }
        [JsonProperty("body")] public string Body { get; set; }
    }
}