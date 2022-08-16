using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.MessageDataModes.Models
{
    public class BufferSummary
    {
        [JsonConstructor]
        public BufferSummary(DateTime buildStartedAt, DateTime buildEndedAt, ulong totalSize, List<Error> buildErrors,
            BuildStatus result, BuildPlatform platform, string outputPath, string version, string productName)
        {
            BuildStartedAt = buildStartedAt;
            TotalSize = totalSize;
            Result = result;
            Platform = platform;
            BuildEndedAt = buildEndedAt;
            BuildErrors = buildErrors;
            OutputPath = outputPath;
            Version = version;
            ProductName = productName;
        }

        [JsonProperty("outputPath")] public string OutputPath { get; }
        [JsonProperty("buildStartedAt")] public DateTime BuildStartedAt { get; }
        [JsonIgnore] public int TotalErrors => BuildErrors?.Count ?? 0;
        [JsonProperty("totalErrors")] public ulong TotalSize { get; private set; }
        [JsonProperty("totalSize")] public BuildStatus Result { get; }
        [JsonProperty("result")] public BuildPlatform Platform { get; }
        [JsonProperty("platform")] public DateTime BuildEndedAt { get; private set; }
        [JsonProperty("buildEndedAt")] public List<Error> BuildErrors { get; private set; }
        [JsonProperty("version")] public string Version { get; }
        [JsonProperty("productName")] public string ProductName { get; }
        
        
        
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
                "Application.productName",
                "Application.version"
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