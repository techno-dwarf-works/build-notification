using System.Net.Http;

namespace BuildNotification.Runtime.Tooling.Interfaces
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