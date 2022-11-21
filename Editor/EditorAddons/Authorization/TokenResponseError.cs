﻿using System;
using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.Authorization
{
    [Serializable]
    public class TokenResponseError
    {
        [JsonConstructor]
        public TokenResponseError(string error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        [JsonProperty("error")] public string Error { get; }
        [JsonProperty("error_description")] public string ErrorDescription { get; }
    }
}