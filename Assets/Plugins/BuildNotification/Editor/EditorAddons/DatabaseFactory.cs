using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.DatabaseModule;
using BuildNotification.EditorAddons.DatabaseModule.RequestWrappers;
using BuildNotification.EditorAddons.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildNotification.EditorAddons
{
    public static class DatabaseFactory
    {
        public static async Task SendToDatabase<TRequest, TResponse, TError>(ISendData messagingData,
            List<TRequest> list)
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
                    instance = new SingleDatabase(messagingData);
                    dataWrapper = new SingleWrapper<TRequest>(list[0]);
                    break;
                default:
                    instance = new BatchDatabase(messagingData);
                    dataWrapper = new ListWrapper<TRequest>(list);
                    break;
            }

            var (respondMessageBody, exception) =
                await instance.PostAsync<TRequest, TResponse, TError>(dataWrapper);

            if (exception == null)
            {
                OnComplete(respondMessageBody);
                return;
            }

            OnError(exception);
        }

        private static void OnError<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj, Formatting.Indented);

            var exception = new HttpRequestException(str);
            Debug.LogException(exception);
        }

        private static void OnComplete<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj, Formatting.Indented);

            Debug.Log(str);
        }
    }
}