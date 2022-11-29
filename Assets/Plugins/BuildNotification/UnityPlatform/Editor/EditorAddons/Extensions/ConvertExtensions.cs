using System;
using System.Collections.Generic;
using Better.BuildNotification.Runtime.MessageData;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons.Extensions
{
    internal static class ConvertExtensions
    {
        public static BufferSummary ToBufferSummary(this BuildReport buildReport)
        {
            var buildSummary = buildReport.summary;
            return new BufferSummary(
                buildSummary.buildStartedAt,
                buildSummary.buildEndedAt,
                buildSummary.totalSize,
                new List<Error>(),
                buildSummary.result.ToBuildStatus(),
                buildSummary.platform.ToBuildPlatform(),
                buildSummary.outputPath,
                Application.version,
                Application.productName
            );
        }
        public static BuildStatus ToBuildStatus(this BuildResult buildResult)
        {
            switch (buildResult)
            {
                case BuildResult.Unknown:
                    return BuildStatus.Unknown;
                case BuildResult.Succeeded:
                    return BuildStatus.Succeeded;
                case BuildResult.Failed:
                    return BuildStatus.Failed;
                case BuildResult.Cancelled:
                    return BuildStatus.Cancelled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildResult), buildResult, null);
            }
        }

        public static BuildPlatform ToBuildPlatform(this BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    return BuildPlatform.StandaloneOSX;
                case BuildTarget.StandaloneWindows:
                    return BuildPlatform.StandaloneWindows;
                case BuildTarget.iOS:
                    return BuildPlatform.iOS;
                case BuildTarget.Android:
                    return BuildPlatform.Android;
                case BuildTarget.StandaloneWindows64:
                    return BuildPlatform.StandaloneWindows64;
                case BuildTarget.WebGL:
                    return BuildPlatform.WebGL;
                case BuildTarget.WSAPlayer:
                    return BuildPlatform.WSAPlayer;
                case BuildTarget.StandaloneLinux64:
                    return BuildPlatform.StandaloneLinux64;
                case BuildTarget.PS4:
                    return BuildPlatform.PS4;
                case BuildTarget.XboxOne:
                    return BuildPlatform.XboxOne;
                case BuildTarget.tvOS:
                    return BuildPlatform.tvOS;
                case BuildTarget.Switch:
                    return BuildPlatform.Switch;
                case BuildTarget.Lumin:
                    return BuildPlatform.Lumin;
                case BuildTarget.Stadia:
                    return BuildPlatform.Stadia;
                case BuildTarget.CloudRendering:
                    return BuildPlatform.CloudRendering;
                case BuildTarget.GameCoreXboxOne:
                    return BuildPlatform.GameCoreXboxOne;
                case BuildTarget.PS5:
                    return BuildPlatform.PS5;
#if UNITY_2021_3_OR_NEWER
                case BuildTarget.EmbeddedLinux:
                    return BuildPlatform.EmbeddedLinux;
#endif
                case BuildTarget.NoTarget:
                    return BuildPlatform.NoTarget;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildTarget), buildTarget, null);
            }
        }
    }
}