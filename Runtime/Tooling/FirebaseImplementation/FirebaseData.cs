using System;
using System.Threading.Tasks;
using BetterAttributes.Runtime.Attributes.ReadOnly;
using BuildNotification.Runtime.Services;
using BuildNotification.Runtime.Tooling.Authorization;
using UnityEngine;

namespace BuildNotification.Runtime.Tooling.FirebaseImplementation
{
    [Serializable]
    public class FirebaseData
    {
        [SerializeField] public CloudMessagingData cloudMessagingData;
        [SerializeField] public RealtimeDatabaseData realtimeDatabaseData;
        [ReadOnlyField] [SerializeField] public FirebaseAdminSDKData serviceAccountData;
        [ReadOnlyField] [SerializeField] public long expirationTime;
        [ReadOnlyField] [SerializeField] public bool lastRequestSuccessful;
        
        public static async Task<bool> ValidateToken()
        {
            var firebaseData = FirebaseDataLoader.Instance.GetData();

            if (firebaseData == null)
            {
                Debug.LogError($"{nameof(FirebaseData)} missing");
                return false;
            }

            var now = DateTimeOffset.Now;
            if (!FirebaseUpdater.ValidateLastRequest(firebaseData, now))
            {
                Debug.Log("Token not valid any more, or last request not successful. Refreshing...");
                return await FirebaseUpdater.RefreshToken(firebaseData, now);
            }

            return true;
        }

        public static RealtimeDatabaseData GetRealtimeDatabaseData()
        {
            var firebaseData = FirebaseDataLoader.Instance.GetData();
            if (firebaseData != null)
            {
                if (firebaseData.serviceAccountData == null || !firebaseData.serviceAccountData.IsValid)
                {
                    Debug.LogError($"Try import service_account_data{PathService.JsonExtensionWithDot} again");
                    return null;
                }

                return firebaseData.realtimeDatabaseData;
            }

            Debug.LogError($"{nameof(FirebaseData)}.asset missing");
            return null;
        }

        public static CloudMessagingData GetCloudMessagingData()
        {
            var fcmScriptable = FirebaseDataLoader.Instance.GetData();
            if (fcmScriptable != null)
            {
                if (fcmScriptable.serviceAccountData == null || !fcmScriptable.serviceAccountData.IsValid)
                {
                    Debug.LogError($"Try import service_account_data{PathService.JsonExtensionWithDot} again");
                    return null;
                }

                return fcmScriptable.cloudMessagingData;
            }

            Debug.LogError($"{nameof(FirebaseData)}.asset missing");
            return null;
        }

        public FirebaseAdminSDKData GetServiceAccountData()
        {
            return serviceAccountData;
        }
    }
}