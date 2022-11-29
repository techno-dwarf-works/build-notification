using System.IO;
using Better.BuildNotification.Runtime.Authorization;
using UnityEngine;

namespace Better.BuildNotification.Runtime.Services
{
    public static class PathService
    {
        public static string ServiceInfoDataPath { get; }
        public const string JsonExtension = "json";
        public const string JsonExtensionWithDot = ".json";
        public const string JsonFileType = "application/json";

        static PathService()
        {
            ServiceInfoDataPath = Path.Combine(Application.persistentDataPath,
                $"{nameof(ServiceInfoData)}{JsonExtensionWithDot}");
        }
    }
}