using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Better.BuildNotification.Platform.Services;
using Better.BuildNotification.Runtime.Services;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class RealtimeDatabaseData : ISendData
    {
        private string _urlBase = "https://{0}-default-rtdb.firebaseio.com/";

        private string[] _scope = new string[]
            { "https://www.googleapis.com/auth/firebase.database", "https://www.googleapis.com/auth/userinfo.email" };

        [JsonProperty("storedBaseUrl")] private string _storedBaseUrl;
        [JsonProperty("token")] private string _token;

        public string[] Scopes => _scope;
        public string BaseUrl => _storedBaseUrl;

        public string RequestUrl => $"/{FirebaseDefinition.MessagesRoot}";
        public string RequestBatchUrl { get; }

        [JsonConstructor]
        public RealtimeDatabaseData([JsonProperty("storedBaseUrl")] string storedBaseUrl,
            [JsonProperty("token")] string token)
        {
            _storedBaseUrl = storedBaseUrl;
            _token = token;
        }

        public RealtimeDatabaseData()
        {
            _storedBaseUrl =string.Empty;
            _token = string.Empty;
        }

        public void SetProject(string project)
        {
            _storedBaseUrl = new Uri(string.Format(_urlBase, project)).ToString();
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