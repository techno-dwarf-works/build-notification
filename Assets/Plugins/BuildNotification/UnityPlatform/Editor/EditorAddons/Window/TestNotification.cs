using System;
using System.Collections.Generic;
using Better.BuildNotification.Platform.Tooling;
using Better.BuildNotification.Runtime.MessageData;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Window
{
    public static class TestNotification
    {
        public static async void TestSucceed(FirebaseData fcmScriptable, Action onComplete)
        {
            var list = new List<MessagingRequest>();

            foreach (var receiver in fcmScriptable.Receivers)
            {
                var data = new FirebaseMessageData(BufferSummary.CreateBufferSummary(BuildStatus.Succeeded));
                var request = new MessagingRequestBody(data, receiver);

                var requestMessage = new MessagingRequest(request);
                list.Add(requestMessage);
            }


            var now = DateTimeOffset.Now;
            if (!FirebaseUpdater.ValidateLastRequest(fcmScriptable, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseUpdater.RefreshToken(fcmScriptable, now)) return;
            }

            await DatabaseFactory.Send<MessagingRequest, MessagingRespondBody, ResponseError>(
                fcmScriptable.MessagingData, list);
            onComplete?.Invoke();
        }

        public static async void TestFailed(FirebaseData fcmScriptable, Action onComplete)
        {
            var list = new List<MessagingRequest>();

            foreach (var receiver in fcmScriptable.Receivers)
            {
                var data = new FirebaseMessageData(BufferSummary.CreateBufferSummary(BuildStatus.Failed));
                var request = new MessagingRequestBody(data, receiver);

                var requestMessage = new MessagingRequest(request);
                list.Add(requestMessage);
            }


            var now = DateTimeOffset.Now;
            if (!FirebaseUpdater.ValidateLastRequest(fcmScriptable, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseUpdater.RefreshToken(fcmScriptable, now)) return;
            }

            await DatabaseFactory.Send<MessagingRequest, MessagingRespondBody, ResponseError>(
                fcmScriptable.MessagingData, list);
            onComplete?.Invoke();
        }
    }
}