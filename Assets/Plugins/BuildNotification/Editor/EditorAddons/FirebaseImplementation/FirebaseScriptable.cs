using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterAttributes.Runtime.EditorAddons.ReadOnlyAttributes;
using BuildNotification.EditorAddons.Authorization;
using BuildNotification.EditorAddons.Models;
using BuildNotification.Runtime;
using UnityEngine;

namespace BuildNotification.EditorAddons.FirebaseImplementation
{
    public class FirebaseScriptable : ScriptableObject
    {
        [SerializeField] internal CloudMessagingData cloudMessagingData;
        [SerializeField] internal RealtimeDatabaseData realtimeDatabaseData;
        [ReadOnlyField] [SerializeField] private FirebaseAdminSDKData serviceAccountData;
        [ReadOnlyField] [SerializeField] internal long expirationTime;
        [ReadOnlyField] [SerializeField] internal bool lastRequestSuccessful;

        public List<Receiver> Receivers => cloudMessagingData.Receivers;
        public bool IsValid => serviceAccountData.IsValid;

        internal void SetFirebaseAdminSDk(FirebaseAdminSDKData adminSDKData)
        {
            serviceAccountData = adminSDKData;
            cloudMessagingData.SetProject(adminSDKData.ProjectID);
            realtimeDatabaseData.SetProject(adminSDKData.ProjectID);
        }

        public static async Task<RealtimeDatabaseData> GetRealtimeDatabaseData()
        {
            var fcmScriptable = Resources.Load<FirebaseScriptable>(nameof(FirebaseScriptable));
            if (fcmScriptable != null)
            {
                if (fcmScriptable.serviceAccountData == null || !fcmScriptable.serviceAccountData.IsValid)
                {
                    Debug.LogError($"Try import service_account_data.{PathService.DefaultExtension} again");
                    return null;
                }

                var now = DateTimeOffset.Now;
                if (FirebaseScriptableUpdater.ValidateLastRequest(fcmScriptable, now))
                    return fcmScriptable.realtimeDatabaseData;

                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseScriptableUpdater.RefreshToken(fcmScriptable, now)) return null;

                return fcmScriptable.realtimeDatabaseData;
            }

            Debug.LogError($"{nameof(FirebaseScriptable)}.asset missing in Editor/Resources");
            return null;
        }
        
        public static async Task<CloudMessagingData> GetCloudMessagingData()
        {
            var fcmScriptable = Resources.Load<FirebaseScriptable>(nameof(FirebaseScriptable));
            if (fcmScriptable != null)
            {
                if (fcmScriptable.serviceAccountData == null || !fcmScriptable.serviceAccountData.IsValid)
                {
                    Debug.LogError($"Try import service_account_data.{PathService.DefaultExtension} again");
                    return null;
                }

                var now = DateTimeOffset.Now;
                if (FirebaseScriptableUpdater.ValidateLastRequest(fcmScriptable, now))
                    return fcmScriptable.cloudMessagingData;

                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                if (!await FirebaseScriptableUpdater.RefreshToken(fcmScriptable, now)) return null;

                return fcmScriptable.cloudMessagingData;
            }

            Debug.LogError($"{nameof(FirebaseScriptable)}.asset missing in Editor/Resources");
            return null;
        }

        public FirebaseAdminSDKData GetServiceAccountData()
        {
            return serviceAccountData;
        }
    }
}