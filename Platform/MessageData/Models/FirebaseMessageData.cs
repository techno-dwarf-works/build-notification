using System.Collections.Generic;
using Better.BuildNotification.Platform.MessageData.Interfaces;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.MessageData.Models
{
    internal struct DataKeys
    {
        public const string Body = "body";
        public const string Guid = "guid";
    }

    public class FirebaseMessageData : IMessageData
    {
        public FirebaseMessageData(IDictionary<string, string> notification)
        {
            if (notification.TryGetValue(DataKeys.Body, out var body))
            {
                var bodyData = JsonConvert.DeserializeObject<BufferSummary>(body) ??
                               BufferSummary.CreateBufferSummary(BuildStatus.Unknown);
                Body = bodyData;
            }

            if (notification.TryGetValue(DataKeys.Guid, out var guid))
            {
                Guid = guid;
            }
        }

        public FirebaseMessageData(BufferSummary body)
        {
            Body = body;
            Guid = System.Guid.NewGuid().ToString();
        }

        [JsonConstructor]
        public FirebaseMessageData(string guid, string body)
        {
            Body = JsonConvert.DeserializeObject<BufferSummary>(body);
            Guid = guid;
        }

        [JsonProperty(DataKeys.Body)] public string BodyString => JsonConvert.SerializeObject(Body);
        [JsonIgnore] public BufferSummary Body { get; }
        [JsonProperty(DataKeys.Guid)] public string Guid { get; }
    }
}