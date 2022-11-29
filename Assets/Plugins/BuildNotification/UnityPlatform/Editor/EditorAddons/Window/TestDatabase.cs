using System;
using System.Net.Http;
using Better.BuildNotification.Runtime.MessageData;
using Better.BuildNotification.Runtime.Services;
using Better.BuildNotification.Runtime.Tooling;
using Better.BuildNotification.Runtime.Tooling.FirebaseImplementation;
using Better.BuildNotification.Runtime.Tooling.Models;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public static class TestDatabase
    {
        public static async void SendData(FirebaseScriptable fcmScriptable, Action onComplete)
        {
            var bufferSummary = BufferSummary.CreateBufferSummary(BuildStatus.Failed);
            bufferSummary.BuildErrors.Add(new Error("Some error",""));
            var data = new FirebaseMessageData(bufferSummary);

            if (!await FirebaseData.ValidateToken())
            {
                return;
            }

            await DatabaseFactory.Send<FirebaseMessageData, DatabaseRespondBody, ResponseError>(
                fcmScriptable.Data.DatabaseData, data, HttpMethod.Put, $"{data.Guid}{PathService.JsonExtensionWithDot}");
            onComplete?.Invoke();
        }
    }
}