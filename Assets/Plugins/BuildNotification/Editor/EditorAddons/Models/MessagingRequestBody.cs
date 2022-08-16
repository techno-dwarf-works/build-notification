using System;
using BuildNotification.Runtime.MessageDataModes;
using BuildNotification.Runtime.MessageDataModes.Models;
using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.Models
{
    [Serializable]
    public class MessagingRequestBody
    {
        public MessagingRequestBody(FirebaseMessageData data, Receiver receiver)
        {
            //TODO: Find better way to create title without formatting
            var generator = new DescriptionGenerator(data.Body);
            Notification = new Notification(generator.GenerateClearTitle(), generator.GenerateMessage());
            Data = data;
            Receiver = receiver.Token;
            Condition = null;
            Topic = null;
        }

        public MessagingRequestBody(FirebaseMessageData data, Condition condition)
        {
            var generator = new DescriptionGenerator(data.Body);
            Notification = new Notification(generator.GenerateClearTitle(), generator.GenerateMessage());
            Data = data;
            Condition = condition.Value;
            Receiver = null;
            Topic = null;
        }

        public MessagingRequestBody(FirebaseMessageData data, Topic topic)
        {
            var generator = new DescriptionGenerator(data.Body);
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