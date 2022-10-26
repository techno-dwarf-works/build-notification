using System;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.Authorization;
using UnityEngine;

namespace BuildNotification.EditorAddons.FirebaseImplementation
{
    public static class FirebaseScriptableUpdater
    {
        public static async Task<bool> RefreshToken(FirebaseScriptable fcmScriptable, DateTimeOffset now)
        {
            var firebaseAdminSDKData = fcmScriptable.GetServiceAccountData();
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

        public static bool ValidateLastRequest(FirebaseScriptable fcmScriptable, DateTimeOffset now)
        {
            return now < FirebaseCustomToken.FromSecond(fcmScriptable.expirationTime) &&
                   fcmScriptable.lastRequestSuccessful;
        }
    }
}