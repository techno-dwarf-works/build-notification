using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuildNotification.Runtime.Tooling.DatabaseModule;
using BuildNotification.Runtime.Tooling.DatabaseModule.RequestWrappers;
using BuildNotification.Runtime.Tooling.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildNotification.Runtime.Tooling
{
    public static class DatabaseFactory
    {
        public static async Task Send<TRequest, TResponse, TError>(ISendData messagingData,
            List<TRequest> list, string query = null)
        {
            await Send<TRequest, TResponse, TError>(messagingData, list, HttpMethod.Post, query);
        }
        
        public static async Task Send<TRequest, TResponse, TError>(ISendData messagingData, List<TRequest> list,
            HttpMethod method, string query = null)
        {
            Database instance;
            Wrapper dataWrapper;
            switch (list.Count)
            {
                case 0:
                    return;
#if UNITY_2021_3_OR_NEWER
                case <= 1:
#else
                case 1:
#endif
                    instance = new SingleDatabase(messagingData, method);
                    dataWrapper = new SingleWrapper<TRequest>(list[0]);
                    break;
                default:
                    instance = new BatchDatabase(messagingData, method);
                    dataWrapper = new ListWrapper<TRequest>(list);
                    break;
            }

            var (respondMessageBody, exception) =
                await instance.PostAsync<TRequest, TResponse, TError>(dataWrapper, query);

            if (exception == null)
            {
                OnComplete(respondMessageBody);
                return;
            }

            OnError(exception);
        }
        
        
        public static async Task Send<TRequest, TResponse, TError>(ISendData messagingData, TRequest data,
            HttpMethod method, string query = null)
        {
            var instance = new SingleDatabase(messagingData, method);
            var dataWrapper = new SingleWrapper<TRequest>(data);
            
            var (respondMessageBody, exception) =
                await instance.PostAsync<TRequest, TResponse, TError>(dataWrapper, query);

            if (exception == null)
            {
                OnComplete(respondMessageBody);
                return;
            }

            OnError(exception);
        }

        private static void OnError<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj);

            var exception = new HttpRequestException(str);
            Debug.LogException(exception);
        }

        private static void OnComplete<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj);

            var builder = new StringBuilder();
            builder.AppendLine("Succeed");
            builder.AppendLine(str);
            Debug.Log(builder.ToString());
        }
    }
}