using System;
using System.Collections.Generic;
using BuildNotification.EditorAddons.FirebaseImplementation;
using BuildNotification.EditorAddons.Models;
using BuildNotification.Runtime.MessageDataModes.Models;
using UnityEngine;

namespace BuildNotification.EditorAddons.Window
{
    public static class TestDatabase
    {
        public static async void SendData(FirebaseScriptable fcmScriptable, Action onComplete)
        {
            var data = new FirebaseMessageData(BufferSummary.CreateBufferSummary(BuildStatus.Succeeded));

            var now = DateTimeOffset.Now;
            if (!FirebaseScriptableUpdater.ValidateLastRequest(fcmScriptable, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseScriptableUpdater.RefreshToken(fcmScriptable, now)) return;
            }
            await DatabaseFactory.SendToDatabase<FirebaseMessageData, DatabaseRespondBody, DatabaseError>(fcmScriptable.realtimeDatabaseData, new List<FirebaseMessageData>(){data});
            onComplete?.Invoke();
        }
    }
}