using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    public class TokenRequest : ISendData
    {
        public TokenRequest(string grantType, string url)
        {
            GrantType = grantType;
            BaseUrl = url;
        }

        [JsonProperty("grant_type")] public string GrantType { get; }

        [JsonProperty("assertion")] public string Assertion { get; private set; }

        public string[] Scopes { get; }

        [JsonIgnore]
        public string BaseUrl { get; }

        [JsonIgnore] public string RequestUrl=> $"?grant_type={GrantType}&assertion={GenerateToken()}";
        [JsonIgnore] public string RequestBatchUrl { get; }


        public void SetToken(string newToken)
        {
            Assertion = newToken;
        }

        public string GenerateToken()
        {
            return Assertion;
        }

        public string GetContentType()
        {
            return "application/x-www-form-urlencoded";
        }

        public void GenerateHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}