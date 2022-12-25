using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class FirebaseData
    {
        [JsonProperty("cloudMessagingData")] private CloudMessagingData _cloudMessagingData;
        [JsonProperty("realtimeDatabaseData")] private RealtimeDatabaseData _realtimeDatabaseData;
        [JsonProperty("firebaseAdminSDKData")] private FirebaseAdminSDKData _firebaseAdminSDKData;
        [JsonProperty("expirationTime")] private long _expirationTime;

        [JsonProperty("lastRequestSuccessful")]
        private bool _lastRequestSuccessful;

        [JsonConstructor]
        public FirebaseData([JsonProperty("cloudMessagingData")] CloudMessagingData cloudMessagingData,
            [JsonProperty("realtimeDatabaseData")] RealtimeDatabaseData realtimeDatabaseData,
            [JsonProperty("firebaseAdminSDKData")] FirebaseAdminSDKData firebaseAdminSDKData,
            [JsonProperty("expirationTime")] long expirationTime,
            [JsonProperty("lastRequestSuccessful")]
            bool lastRequestSuccessful)
        {
            _cloudMessagingData = cloudMessagingData;
            _realtimeDatabaseData = realtimeDatabaseData;
            _firebaseAdminSDKData = firebaseAdminSDKData;
            _expirationTime = expirationTime;
            _lastRequestSuccessful = lastRequestSuccessful;
        }

        public FirebaseData()
        {
            _cloudMessagingData = new CloudMessagingData();
            _realtimeDatabaseData = new RealtimeDatabaseData();
            _firebaseAdminSDKData = new FirebaseAdminSDKData();
            _expirationTime = 0;
            _lastRequestSuccessful = false;
        }

        public FirebaseAdminSDKData FirebaseAdminSDKData
        {
            get => _firebaseAdminSDKData;
            set => _firebaseAdminSDKData = value;
        }

        public CloudMessagingData MessagingData => _cloudMessagingData;

        public RealtimeDatabaseData DatabaseData => _realtimeDatabaseData;

        public long ExpirationTime
        {
            get => _expirationTime;
            set => _expirationTime = value;
        }

        public bool LastRequestSuccessful
        {
            get => _lastRequestSuccessful;
            set => _lastRequestSuccessful = value;
        }

        public static async Task<bool> ValidateToken()
        {
            var firebaseData = FirebaseDataLoader.Instance.GetData();

            if (firebaseData == null)
            {
                FirebaseLogger.Instance.LogError($"{nameof(FirebaseData)} missing");
                return false;
            }

            if (!firebaseData.IsValid)
            {
                FirebaseLogger.Instance.LogError(
                    $"{nameof(FirebaseData)} is not valid. Try reimport {nameof(Better.BuildNotification.Platform.Tooling.FirebaseAdminSDKData)}.");
                return false;
            }

            var now = DateTimeOffset.Now;
            if (!FirebaseUpdater.ValidateLastRequest(firebaseData, now))
            {
                FirebaseLogger.Instance.Log("Token not valid any more, or last request not successful. Refreshing...");
                return await FirebaseUpdater.RefreshToken(firebaseData, now);
            }

            return true;
        }

        public static RealtimeDatabaseData GetFirebaseAdminSDKData()
        {
            var firebaseData = FirebaseDataLoader.Instance.GetData();
            if (firebaseData != null)
            {
                if (firebaseData._firebaseAdminSDKData == null || !firebaseData._firebaseAdminSDKData.IsValid)
                {
                    FirebaseLogger.Instance.Log(
                        $"{nameof(FirebaseData)} is not valid. Try reimport {nameof(Better.BuildNotification.Platform.Tooling.FirebaseAdminSDKData)}.");
                    return null;
                }

                return firebaseData._realtimeDatabaseData;
            }

            FirebaseLogger.Instance.LogError($"{nameof(FirebaseData)}{FirebaseDataLoader.AssetExtensionWithDot} missing");
            return null;
        }

        public static CloudMessagingData GetCloudMessagingData()
        {
            var fcmScriptable = FirebaseDataLoader.Instance.GetData();
            if (fcmScriptable != null)
            {
                if (fcmScriptable._firebaseAdminSDKData == null || !fcmScriptable._firebaseAdminSDKData.IsValid)
                {
                    FirebaseLogger.Instance.Log(
                        $"{nameof(FirebaseData)} is not valid. Try reimport {nameof(Better.BuildNotification.Platform.Tooling.FirebaseAdminSDKData)}.");
                    return null;
                }

                return fcmScriptable._cloudMessagingData;
            }

            FirebaseLogger.Instance.LogError($"{nameof(FirebaseData)}{FirebaseDataLoader.AssetExtensionWithDot} missing");
            return null;
        }

        public bool IsValid => FirebaseAdminSDKData.IsValid;
        public List<Receiver> Receivers => MessagingData.Receivers;

        public void SetFirebaseAdminSDk(FirebaseAdminSDKData adminSDKData)
        {
            FirebaseAdminSDKData = adminSDKData;
            MessagingData.SetProject(adminSDKData.ProjectID);
            DatabaseData.SetProject(adminSDKData.ProjectID);
        }

        public FirebaseAdminSDKData GetServiceAccountData()
        {
            return _firebaseAdminSDKData;
        }
    }
}