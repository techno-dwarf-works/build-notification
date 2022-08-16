using System;
using System.Collections.Generic;
using BuildNotification.EditorAddons.FirebaseImplementation;
using BuildNotification.EditorAddons.Models;
using BuildNotification.Runtime.MessageDataModes.Models;
using UnityEngine;

namespace BuildNotification.EditorAddons.Window
{
    public static class TestNotification
    {
        public static async void TestSucceed(FirebaseScriptable fcmScriptable, Action onComplete)
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
            if (!FirebaseScriptableUpdater.ValidateLastRequest(fcmScriptable, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseScriptableUpdater.RefreshToken(fcmScriptable, now)) return;
            }

            await DatabaseFactory.SendToDatabase<MessagingRequest, MessagingRespondBody, Exception>(
                fcmScriptable.cloudMessagingData, list);
            onComplete?.Invoke();
        }

        public static async void TestFailed(FirebaseScriptable fcmScriptable, Action onComplete)
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
            if (!FirebaseScriptableUpdater.ValidateLastRequest(fcmScriptable, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseScriptableUpdater.RefreshToken(fcmScriptable, now)) return;
            }

            await DatabaseFactory.SendToDatabase<MessagingRequest, MessagingRespondBody, Exception>(
                fcmScriptable.cloudMessagingData, list);
            onComplete?.Invoke();
        }
    }
}