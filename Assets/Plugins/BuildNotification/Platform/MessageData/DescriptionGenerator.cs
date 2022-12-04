using System;
using System.Text;
using Better.BuildNotification.Platform.MessageData.Models;
using Better.BuildNotification.Platform.Services;

namespace Better.BuildNotification.Platform.MessageData
{
    public class DescriptionGenerator
    {
        private int skipLastErrors = 3;

        private BufferSummary _summary;
        
        public void Set(BufferSummary summary)
        {
            _summary = summary;
        }

        public string GenerateBody()
        {
            var str = new StringBuilder();
            if (_summary.BuildStatus == BuildStatus.Succeeded)
                str.AppendLine($"Build path: {_summary.OutputPath}");
            return str.ToString();
        }

        public string GenerateMessage()
        {
            var str = new StringBuilder();
            var totalBuildTime = TotalBuildTime();
            str.AppendLine(
                $"Build completed with a result of '{_summary.BuildStatus}' in {totalBuildTime:hh\\:mm\\:ss} ({totalBuildTime.TotalMilliseconds:####} ms)");
            if (_summary.BuildStatus == BuildStatus.Succeeded)
                str.AppendLine($"Build size: {_summary.TotalSize.ToMegabytes():F} mb");

            return str.ToString();
        }

        public string GenerateFormattedTitle()
        {
            return
                $"Build for project <b>{_summary.ProjectName}:v{_summary.Version}</b> Platform: <b>{_summary.Platform}</b>";
        }
        
        public string GenerateClearTitle()
        {
            return
                $"Build for project {_summary.ProjectName}:v{_summary.Version} Platform: {_summary.Platform}";
        }

        public TimeSpan TotalBuildTime()
        {
            return (_summary.BuildEndedAt - _summary.BuildStartedAt);
        }

        public string GenerateTimestamp()
        {
            var localTime = _summary.BuildEndedAt.ToLocalTime();
            return $"{localTime.ToShortDateString()} [{localTime.ToLongTimeString()}]";
        }
    }
}