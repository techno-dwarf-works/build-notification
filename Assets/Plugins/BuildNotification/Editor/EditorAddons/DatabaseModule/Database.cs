using System;
using System.Net.Http;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.DatabaseModule.RequestWrappers;
using BuildNotification.EditorAddons.Interfaces;
using UnityEngine;

namespace BuildNotification.EditorAddons.DatabaseModule
{
    public abstract class Database : IDisposable
    {
        private protected readonly ISendData _sendData;
        private protected readonly HttpMethod _sendMethod;

        protected Database(ISendData cmData)
        {
            _sendData = cmData;
            _sendMethod = HttpMethod.Post;
        }

        protected Database(ISendData cmData, HttpMethod method)
        {
            _sendData = cmData;
            _sendMethod = method;
        }

        /// <summary>
        /// Create data in the database by path
        /// </summary>
        /// <param name="request"></param>
        /// <param name="query"></param>
        public async Task<(Wrapper, Wrapper)> PostAsync<TRequest, TResponse, TError>(Wrapper request,
            string query = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_sendData.BaseUrl);
                var content = GenerateContent<TRequest>(request);
                _sendData.GenerateHeaders(client);

                var responseMessage = await SendRequest(client, content, query);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var respond = await ParseResponseMessage<TResponse>(responseMessage);
                    return (respond, default);
                }

                try
                {
                    var error = await ParseResponseMessage<TError>(responseMessage);
                    return (default, error);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return (default, null);
                }
            }
        }

        private protected string GenerateUri(string requestUrl, string query)
        {
            if (!string.IsNullOrEmpty(requestUrl) && !string.IsNullOrEmpty(query))
            {
                return $"{requestUrl}/{query}";
            }

            if (!string.IsNullOrEmpty(requestUrl))
            {
                return $"{requestUrl}";
            }

            if (!string.IsNullOrEmpty(query))
            {
                return $"{query}";
            }

            return null;
        }

        private protected abstract Task<Wrapper> ParseResponseMessage<TResponse>(HttpResponseMessage responseMessage);

        private protected abstract Task<HttpResponseMessage> SendRequest(HttpClient client, HttpContent content,
            string query = null);

        private protected abstract HttpContent GenerateContent<TRequest>(Wrapper requestWrapper);

        public void Dispose()
        {
        }
    }
}