using System;
using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.Models
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