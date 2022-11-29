using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Better.BuildNotification.Runtime.MessageDataModes
{
    public class BufferSummary
    {
        [JsonConstructor]
        public BufferSummary(DateTime buildStartedAt, DateTime buildEndedAt, ulong totalSize, List<Error> buildErrors,
            BuildStatus buildStatus, BuildPlatform platform, string outputPath, string version, string productName)
        {
            BuildStartedAt = buildStartedAt;
            TotalSize = totalSize;
            BuildStatus = buildStatus;
            Platform = platform;
            BuildEndedAt = buildEndedAt;
            BuildErrors = buildErrors;
            OutputPath = outputPath;
            Version = version;
            ProjectName = productName;
        }

        [JsonProperty("outputPath")] public string OutputPath { get; }
        [JsonProperty("buildStartedAt")] public DateTime BuildStartedAt { get; }
        [JsonIgnore] public int TotalErrors => BuildErrors?.Count ?? 0;
        [JsonProperty("totalSize")] public ulong TotalSize { get; private set; }
        [JsonProperty("buildStatus")] public BuildStatus BuildStatus { get; }
        [JsonProperty("platform")] public BuildPlatform Platform { get; }
        [JsonProperty("buildEndedAt")] public DateTime BuildEndedAt { get; private set; }
        [JsonProperty("buildErrors")] public List<Error> BuildErrors { get; private set; }
        [JsonProperty("version")] public string Version { get; }
        [JsonProperty("productName")] public string ProjectName { get; }


        public static BufferSummary CreateBufferSummary(BuildStatus buildStatus)
        {
            return new BufferSummary(
                DateTime.Now,
                DateTime.Now.AddHours(1),
                0,
                new List<Error>(),
                buildStatus,
                BuildPlatform.Android,
                "buildSummary.outputPath",
                Application.version,
                Application.productName
            );
        }

        public void SetEndedAt(DateTime utcNow)
        {
            BuildEndedAt = utcNow;
        }

        public void SetErrors(List<Error> errorList)
        {
            BuildErrors = errorList;
        }

        public void UpdateBuildSize()
        {
            TotalSize = (ulong)new FileInfo(OutputPath).Length;
        }
    }
}