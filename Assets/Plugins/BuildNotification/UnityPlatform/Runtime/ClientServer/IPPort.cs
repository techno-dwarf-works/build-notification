using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    [Serializable]
    public class IPPort
    {
        [JsonConstructor]
        public IPPort([JsonProperty("ip")] string ip, [JsonProperty("port")] int port)
        {
            IP = ip;
            Port = port;
        }

        [JsonProperty("ip")] public string IP { get; }
        [JsonProperty("port")] public int Port { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}