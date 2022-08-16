using System;
using System.Text;
using BuildNotification.Runtime.MessageDataModes.Models;

namespace BuildNotification.Runtime.MessageDataModes
{
    public class DescriptionGenerator
    {
        private int skipLastErrors = 3;

        private readonly BufferSummary _summary;

        public DescriptionGenerator(BufferSummary summary)
        {
            _summary = summary;
        }

        public string GenerateBody()
        {
            var str = new StringBuilder();
            if (_summary.Result == BuildStatus.Succeeded)
                str.AppendLine($"Build path: {_summary.OutputPath}");
            return str.ToString();
        }

        public string GenerateMessage()
        {
            var str = new StringBuilder();
            var totalBuildTime = TotalBuildTime();
            str.AppendLine(
                $"Build completed with a result of '{_summary.Result}' in {totalBuildTime:hh\\:mm\\:ss} ({totalBuildTime.TotalMilliseconds:####} ms)");
            if (_summary.Result == BuildStatus.Succeeded)
                str.AppendLine($"Build size: {_summary.TotalSize.ToMegabytes():F} mb");

            return str.ToString();
        }

        public string GenerateFormattedTitle()
        {
            return
                $"Build for project <b>{_summary.ProductName}:v{_summary.Version}</b> Platform: <b>{_summary.Platform}</b>";
        }
        
        public string GenerateClearTitle()
        {
            return
                $"Build for project {_summary.ProductName}:v{_summary.Version} Platform: {_summary.Platform}";
        }

        public TimeSpan TotalBuildTime()
        {
            return (_summary.BuildEndedAt - _summary.BuildStartedAt);
        }
    }
}