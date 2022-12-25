using System;
using Better.BuildNotification.Platform.Tooling;
using UnityEditor;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons
{
    [InitializeOnLoad]
    public class UnityLogger : FirebaseLogger
    {
        static UnityLogger()
        {
            _instance = new UnityLogger();
        }
        
        public override void LogError(string error)
        {
            Debug.LogError(error);
        }

        public override void Log(string log)
        {
            Debug.Log(log);
        }

        public override void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}