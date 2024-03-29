﻿using Newtonsoft.Json;

namespace Better.BuildNotification.Runtime.MessageData
{
    public class Error
    {
        [JsonConstructor]
        public Error(string condition, string stacktrace)
        {
            Condition = condition;
            Stacktrace = stacktrace;
        }

        public string Condition { get; }
        public string Stacktrace { get; }
    }
}