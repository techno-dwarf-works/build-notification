using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.DatabaseModule.RequestWrappers;
using BuildNotification.EditorAddons.Interfaces;
using Newtonsoft.Json;

namespace BuildNotification.EditorAddons.DatabaseModule
{
    public class SingleDatabase : Database
    {
        public SingleDatabase(ISendData cmData)
        {
            sendData = cmData;
        }

        private string GetContentType()
        {
            return sendData.GetContentType();
        }

        private protected override async Task<Wrapper> ParseResponseMessage<TResponse>(HttpResponseMessage responseMessage)
        {
            var stringAsync = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(stringAsync);
            return new SingleWrapper<TResponse>(response);
        }

        private protected override async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content)
        {
            return await client.PostAsync(sendData.RequestUrl, content);
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
            else
            {
                throw new InvalidDataException();
            }
        }
    }
}