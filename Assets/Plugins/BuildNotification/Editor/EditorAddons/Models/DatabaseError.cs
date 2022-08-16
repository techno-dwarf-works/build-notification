using System;
using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.Models
{
    [Serializable]
    public class DatabaseError
    {
        [JsonConstructor]
        public DatabaseError(string error)
        {
            Error = error;
        }

        [JsonProperty("error")] public string Error { get; }
    }
}