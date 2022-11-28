using System;
using System.Net.Http;
using BuildNotification.Runtime;
using BuildNotification.Runtime.MessageDataModes.Models;
using BuildNotification.Runtime.Services;
using BuildNotification.Runtime.Tooling;
using BuildNotification.Runtime.Tooling.FirebaseImplementation;
using BuildNotification.Runtime.Tooling.Models;

namespace BuildNotification.Unity.EditorAddons.Window
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
                fcmScriptable.Data.realtimeDatabaseData, data, HttpMethod.Put, $"{data.Guid}{PathService.JsonExtensionWithDot}");
            onComplete?.Invoke();
        }
    }
}