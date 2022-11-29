using System.Threading.Tasks;
using Better.BuildNotification.Runtime.MessageDataModes;
using Better.BuildNotification.Runtime.Tooling;
using Better.BuildNotification.UnityPlatform.EditorAddons.Extensions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform
{
    public class NotificationBuildPostProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; } = 999;

        private BuildPostPostProcessor _processor = new BuildPostPostProcessor();

        public void OnPreprocessBuild(BuildReport report)
        {
            _processor.Clear();
            Application.logMessageReceived += OnBuildError;
            CheckBuildRunning(report);
        }

        private async void CheckBuildRunning(BuildReport report)
        {
            while (BuildPipeline.isBuildingPlayer)
            {
                await Task.Yield();
            }

            var summary = report.ToBufferSummary();
            _processor.UpdateSummary(ref summary);

            _processor.AnalyzeSummary(summary);
            _processor.Clear();
            Application.logMessageReceived -= OnBuildError;
        }

        private void OnBuildError(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Error)
            {
                _processor.AddError(new Error(condition, stacktrace));
            }
        }
    }
}