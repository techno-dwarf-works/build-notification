using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class MessagingRespondBody
    {
        [JsonConstructor]
        public MessagingRespondBody(string name)
        {
            Name = name;
        }

        [JsonProperty("name")] public string Name { get; }
    }
}