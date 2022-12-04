using System;
using Better.BuildNotification.Platform.MessageData;
using Better.BuildNotification.Platform.MessageData.Models;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class MessagingRequestBody
    {
        public MessagingRequestBody(FirebaseMessageData data, Receiver receiver)
        {
            var generator = new DescriptionGenerator();
            generator.Set(data.Body);
            Notification = new Notification(generator.GenerateClearTitle(), generator.GenerateMessage());
            Data = data;
            Receiver = receiver.Token;
            Condition = null;
            Topic = null;
        }

        public MessagingRequestBody(FirebaseMessageData data, Condition condition)
        {
            var generator = new DescriptionGenerator();
            generator.Set(data.Body);
            Notification = new Notification(generator.GenerateClearTitle(), generator.GenerateMessage());
            Data = data;
            Condition = condition.Value;
            Receiver = null;
            Topic = null;
        }

        public MessagingRequestBody(FirebaseMessageData data, Topic topic)
        {
            var generator = new DescriptionGenerator();
            generator.Set(data.Body);
            Notification = new Notification(generator.GenerateClearTitle(), generator.GenerateMessage());
            Data = data;
            Topic = topic.Value;
            Receiver = null;
            Condition = null;
        }

        [JsonProperty("notification")] public Notification Notification { get; }
        [JsonProperty("condition")] public string Condition { get; }
        [JsonProperty("topic")] public string Topic { get; }
        [JsonProperty("token")] public string Receiver { get; }
        [JsonProperty("data")] public FirebaseMessageData Data { get; set; }
    }
}