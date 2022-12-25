using System.Net.Http;

namespace Better.BuildNotification.Platform.Tooling
{
    public interface ISendData
    {
        public string[] Scopes { get; }
        string BaseUrl { get; }
        string RequestUrl { get; }
        string RequestBatchUrl { get; }
        string GetContentType();
        void GenerateHeaders(HttpClient client);
        string GenerateToken();
    }
}