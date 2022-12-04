using System;
using System.Net.Http;
using Better.BuildNotification.Platform.Services;
using Better.BuildNotification.Platform.Tooling;
using Better.BuildNotification.Runtime.MessageData;
using Better.BuildNotification.Runtime.Services;

namespace Better.BuildNotification.UnityPlatform.Editor.EditorAddons.Window
{
    public static class TestDatabase
    {
        public static async void SendData(FirebaseData fcmScriptable, Action onComplete)
        {
            var bufferSummary = BufferSummary.CreateBufferSummary(BuildStatus.Failed);
            bufferSummary.BuildErrors.Add(new Error("Some error",""));
            var data = new FirebaseMessageData(bufferSummary);

            if (!await FirebaseData.ValidateToken())
            {
                return;
            }

            await DatabaseFactory.Send<FirebaseMessageData, DatabaseRespondBody, ResponseError>(
                fcmScriptable.DatabaseData, data, HttpMethod.Put, $"{data.Guid}{PathService.JsonExtensionWithDot}");
            onComplete?.Invoke();
        }
    }
}