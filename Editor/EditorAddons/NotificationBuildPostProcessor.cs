using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuildNotification.EditorAddons.Extensions;
using BuildNotification.EditorAddons.FirebaseImplementation;
using BuildNotification.EditorAddons.Models;
using BuildNotification.Runtime;
using BuildNotification.Runtime.MessageDataModes.Models;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BuildNotification.EditorAddons
{
    public class NotificationBuildPostProcessor : IPreprocessBuildWithReport
    {
        private const string NotSendingCloudNotification = "Not sending cloud notification";
        private const string SendingCloudNotification = "Sending cloud notification";
        public int callbackOrder { get; } = 999;

        private List<Error> errorList = new List<Error>();

        public void OnPreprocessBuild(BuildReport report)
        {
            errorList = new List<Error>();
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
            summary.SetEndedAt(DateTime.UtcNow);
            summary.SetErrors(errorList);
            summary.UpdateBuildSize();

            AnalyzeSummary(summary);
            errorList.Clear();
            Application.logMessageReceived -= OnBuildError;
        }

        private void OnBuildError(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Error)
            {
                errorList.Add(new Error(condition, stacktrace));
            }
        }

        private void AnalyzeSummary(BufferSummary bufferSummary)
        {
            var reportLog = new StringBuilder();
            reportLog.AppendLine($"Build status {bufferSummary.BuildStatus}.");
            switch (bufferSummary.BuildStatus)
            {
                case BuildStatus.Unknown:
                    reportLog.AppendLine(NotSendingCloudNotification);
                    break;
                case BuildStatus.Succeeded:
                case BuildStatus.Failed:
                case BuildStatus.Cancelled:
                    reportLog.AppendLine(SendingCloudNotification);
                    SendCloudMessage(bufferSummary);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log(reportLog);
        }

        private async void SendCloudMessage(BufferSummary summary)
        {
            if (!await FirebaseScriptable.ValidateToken())
            {
                return;
            }
            
            var cloudMessagingData = FirebaseScriptable.GetCloudMessagingData();
            var realtimeDatabaseData = FirebaseScriptable.GetRealtimeDatabaseData();
            if (cloudMessagingData == null || realtimeDatabaseData == null)
            {
                return;
            }

            var list = new List<MessagingRequest>();
            var data = new FirebaseMessageData(summary);

            foreach (var receiver in cloudMessagingData.Receivers)
            {
                var request = new MessagingRequestBody(data, receiver);
                var requestMessage = new MessagingRequest(request);
                list.Add(requestMessage);
            }

            await DatabaseFactory
                .Send<MessagingRequest, MessagingRespondBody, ResponseError>(cloudMessagingData, list);

            await DatabaseFactory.Send<FirebaseMessageData, DatabaseRespondBody, ResponseError>(
                realtimeDatabaseData, data, HttpMethod.Put, $"{data.Guid}{PathService.JsonExtensionWithDot}");
        }
    }
}