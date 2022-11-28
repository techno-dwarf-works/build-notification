using System;
using System.Threading.Tasks;
using BuildNotification.Runtime.Tooling.Authorization;
using UnityEngine;

namespace BuildNotification.Runtime.Tooling.FirebaseImplementation
{
    public static class FirebaseUpdater
    {
        public static async Task<bool> RefreshToken(FirebaseData fcmScriptable, DateTimeOffset now)
        {
            var firebaseAdminSDKData = fcmScriptable.serviceAccountData;
            var (messageToken, messageTokenError) =
                await FirebaseCustomToken.MakeTokenRequest(firebaseAdminSDKData, fcmScriptable.cloudMessagingData.Scopes, now);

            if (messageTokenError != null)
            {
                Debug.LogError(
                    $"Error: {messageTokenError.Error} Error Description: {messageTokenError.ErrorDescription}");
            
                fcmScriptable.lastRequestSuccessful = false;
                return false;
            }
            
            var (databaseToken, databaseTokenError) =
                await FirebaseCustomToken.MakeTokenRequest(firebaseAdminSDKData, fcmScriptable.realtimeDatabaseData.Scopes, now);

            if (databaseTokenError != null)
            {
                Debug.LogError(
                    $"Error: {databaseTokenError.Error} Error Description: {databaseTokenError.ErrorDescription}");

                fcmScriptable.lastRequestSuccessful = false;
                return false;
            }

            fcmScriptable.cloudMessagingData.SetToken(messageToken.AccessToken);
            fcmScriptable.realtimeDatabaseData.SetToken(databaseToken.AccessToken);
            fcmScriptable.expirationTime = FirebaseCustomToken.GetExpirationTime(now);
            fcmScriptable.lastRequestSuccessful = true;
            return true;
        }

        public static bool ValidateLastRequest(FirebaseData fcmScriptable, DateTimeOffset now)
        {
            return now < FirebaseCustomToken.FromSecond(fcmScriptable.expirationTime) &&
                   fcmScriptable.lastRequestSuccessful;
        }
    }
}