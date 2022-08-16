using System;
using System.Net.Http;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.DatabaseModule.RequestWrappers;
using BuildNotification.EditorAddons.Interfaces;

namespace BuildNotification.EditorAddons.DatabaseModule
{
    public abstract class Database : IDisposable
    {
        private protected ISendData sendData;

        /// <summary>
        /// Create data in the database by path
        /// </summary>
        /// <param name="request"></param>
        public async Task<(Wrapper, Wrapper)> PostAsync<TRequest, TResponse, TError>(Wrapper request) {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sendData.BaseUrl);
                var content = GenerateContent<TRequest>(request);
                sendData.GenerateHeaders(client);

                var responseMessage = await SendRequest(client, content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var respond = await ParseResponseMessage<TResponse>(responseMessage);
                    return (respond, default);
                }

                var error = await ParseResponseMessage<TError>(responseMessage);

                return (default, error);
            }
        }

        private protected abstract Task<Wrapper> ParseResponseMessage<TResponse>(HttpResponseMessage responseMessage);

        private protected abstract Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content);

        private protected abstract HttpContent GenerateContent<TRequest>(Wrapper requestWrapper);

        public void Dispose()
        {
        }
    }
}