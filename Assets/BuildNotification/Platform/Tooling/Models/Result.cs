﻿using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class Result
    {
        [JsonConstructor]
        public Result(string messageID, string error)
        {
            MessageID = messageID;
            Error = error;
        }

        [JsonProperty("message_id")] public string MessageID { get; }
        [JsonProperty("error")] public string Error { get; }
    }
}