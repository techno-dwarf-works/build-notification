using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Better.BuildNotification.Platform.Services;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class CloudMessagingData : ISendData
    {
        private string _urlBase = "https://fcm.googleapis.com";
        private string _requestUrlBase = "/v1/projects/{0}/messages:send";
        private string _batchUrlBase = "/batch";

        private string[] _scope = new string[]
            { "https://www.googleapis.com/auth/firebase.messaging" };

        [JsonProperty("storedRequestUrl")] private string _storedRequestUrl;
        [JsonProperty("token")] private string _token;
        [JsonProperty("receiver")] private List<string> _receivers;

        [JsonConstructor]
        public CloudMessagingData([JsonProperty("storedRequestUrl")] string storedRequestUrl,
            [JsonProperty("token")] string token,
            [JsonProperty("receiver")] List<string> receivers)
        {
            _storedRequestUrl = storedRequestUrl;
            _token = token;
            _receivers = receivers;
        }

        public CloudMessagingData()
        {
            _storedRequestUrl = string.Empty;
            _token = string.Empty;
            _receivers = new List<string>();
        }

        public string BaseUrl => _urlBase;

        public string RequestUrl => _storedRequestUrl;

        public string RequestBatchUrl => _batchUrlBase;

        public List<Receiver> Receivers => _receivers.Select(x => new Receiver(x)).ToList();

        public string[] Scopes => _scope;

        public void SetProject(string project)
        {
            _storedRequestUrl = string.Format(_requestUrlBase, project);
        }

        public string GetContentType()
        {
            return PathService.JsonFileType;
        }

        public void SetToken(string newToken)
        {
            _token = newToken;
        }

        public string GenerateToken()
        {
            return _token;
        }

        public void GenerateHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GenerateToken());
        }
    }
}