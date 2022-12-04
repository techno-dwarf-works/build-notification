using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Better.BuildNotification.Platform.Tooling.RequestWrappers;
using Newtonsoft.Json;
using HttpMessageContent = Better.BuildNotification.Platform.Tooling.Extensions.HttpMessageContent;

namespace Better.BuildNotification.Platform.Tooling
{
    public class BatchDatabase : Database
    {
        private const string Boundary = "subrequestboundary";

        public BatchDatabase(ISendData cmData) : base(cmData)
        {
        }

        public BatchDatabase(ISendData cmData, HttpMethod method) : base(cmData, method)
        {
        }

        private protected override async Task<Wrapper> ParseResponseMessage<TResponse>(
            HttpResponseMessage responseMessage)
        {
            var parsed = await responseMessage.Content.ReadAsStringAsync();
#if UNITY_2021_3_OR_NEWER
                var matches = Regex.Matches(parsed, @"{({*[^{}]*}*)}").ToList();
#else
            var matches = Regex.Matches(parsed, @"{({*[^{}]*}*)}").Cast<object>().ToList();
#endif

            var respondList = matches.Select(match => JsonConvert.DeserializeObject<TResponse>(match.ToString()))
                .ToList();

            return new ListWrapper<TResponse>(respondList);
        }

        private protected override async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content, string query = null)
        {
            var uri = GenerateUri(_sendData.RequestBatchUrl, query);
            return await client.PostAsync(uri, content);
        }

        private protected override HttpContent GenerateContent<TRequest>(Wrapper requestWrapper)
        {
            if (!(requestWrapper is ListWrapper<TRequest> list)) throw new InvalidDataException();
            var content = new MultipartContent("mixed", Boundary);
            foreach (var request in list.Data)
            {
                var serializeObject = JsonConvert.SerializeObject(request, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                var requestMessage = new HttpRequestMessage(HttpMethod.Post,
                    new Uri(new Uri(_sendData.BaseUrl), _sendData.RequestUrl))
                {
                    Content = new StringContent(serializeObject)
                };

                var messageContent = new HttpMessageContent(
                    requestMessage
                );


                messageContent.Headers.ContentType = new MediaTypeHeaderValue(_sendData.GetContentType());
                messageContent.Headers.TryAddWithoutValidation("Content-Transfer-Encoding", "binary");
                messageContent.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_sendData.GenerateToken()}");
                messageContent.Headers.TryAddWithoutValidation("Accept", _sendData.GetContentType());

                content.Add(messageContent);
            }

            return content;
        }
    }
}