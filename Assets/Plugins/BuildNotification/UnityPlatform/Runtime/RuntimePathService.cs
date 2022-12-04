using System.IO;
using Better.BuildNotification.Platform.Services;
using Better.BuildNotification.Runtime.Authorization;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.Runtime
{
    public static class RuntimePathService
    {
        public static string ServiceInfoDataPath { get; }

        static RuntimePathService()
        {
            ServiceInfoDataPath = Path.Combine(Application.persistentDataPath,
                $"{nameof(ServiceInfoData)}{PathService.JsonExtensionWithDot}");
        }
    }
}