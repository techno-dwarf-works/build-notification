using System.Collections.Generic;
using Better.BuildNotification.Runtime.Tooling.Authorization;
using Better.BuildNotification.Runtime.Tooling.FirebaseImplementation;
using Better.BuildNotification.Runtime.Tooling.Models;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons
{
    public class FirebaseScriptable : ScriptableObject
    {
        [SerializeField] private FirebaseData data;

        public List<Receiver> Receivers => data.MessagingData.Receivers;
        public bool IsValid => data.ServiceAccountData.IsValid;

        public FirebaseData Data => data;

        internal void SetFirebaseAdminSDk(FirebaseAdminSDKData adminSDKData)
        {
            data.ServiceAccountData = adminSDKData;
            data.MessagingData.SetProject(adminSDKData.ProjectID);
            data.DatabaseData.SetProject(adminSDKData.ProjectID);
        }
    }
}