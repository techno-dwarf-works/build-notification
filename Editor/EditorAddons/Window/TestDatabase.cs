using System;
using System.Collections.Generic;
using System.Net.Http;
using BuildNotification.EditorAddons.FirebaseImplementation;
using BuildNotification.EditorAddons.Models;
using BuildNotification.Runtime;
using BuildNotification.Runtime.MessageDataModes.Models;
using UnityEngine;

namespace BuildNotification.EditorAddons.Window
{
    public static class TestDatabase
    {
        public static async void SendData(FirebaseScriptable fcmScriptable, Action onComplete)
        {
            var bufferSummary = BufferSummary.CreateBufferSummary(BuildStatus.Failed);
            bufferSummary.BuildErrors.Add(new Error("Some error",""));
            var data = new FirebaseMessageData(bufferSummary);

            if (!await FirebaseScriptable.ValidateToken())
            {
                return;
            }

            await DatabaseFactory.Send<FirebaseMessageData, DatabaseRespondBody, ResponseError>(
                fcmScriptable.realtimeDatabaseData, data, HttpMethod.Put, $"{data.Guid}{PathService.JsonExtensionWithDot}");
            onComplete?.Invoke();
        }
    }
}