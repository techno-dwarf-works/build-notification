using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class ResponseError
    {
        [JsonConstructor]
        public ResponseError(string error)
        {
            Error = error;
        }

        [JsonProperty("error")] public string Error { get; }
    }
}