using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Better.BuildNotification.Platform.Tooling.RequestWrappers;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    public class SingleDatabase : Database
    {
        public SingleDatabase(ISendData cmData) : base(cmData)
        {
        }

        public SingleDatabase(ISendData cmData, HttpMethod method) : base(cmData, method)
        {
        }
        
        private string GetContentType()
        {
            return _sendData.GetContentType();
        }

        private protected override async Task<Wrapper> ParseResponseMessage<TResponse>(
            HttpResponseMessage responseMessage)
        {
            var stringAsync = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(stringAsync);
            return new SingleWrapper<TResponse>(response);
        }

        private protected override async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content,
            string query = null)
        {
            var uri = GenerateUri(_sendData.RequestUrl, query);
            var httpRequestMessage = new HttpRequestMessage(_sendMethod, uri)
            {
                Content = content
            };
            return await client.SendAsync(httpRequestMessage);
        }

        private protected override HttpContent GenerateContent<TRequest>(Wrapper requestWrapper)
        {
            if (requestWrapper is SingleWrapper<TRequest> wrapper)
            {
                var serializeObject = JsonConvert.SerializeObject(wrapper.Data, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                return new StringContent(serializeObject, Encoding.UTF8, GetContentType());
            }

            throw new InvalidDataException();
        }
    }
}