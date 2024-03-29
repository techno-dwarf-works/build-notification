﻿using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class DatabaseRespondBody
    {
        [JsonConstructor]
        public DatabaseRespondBody(string name)
        {
            Name = name;
        }

        [JsonProperty("name")] public string Name { get; }
    }
}