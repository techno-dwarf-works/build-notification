using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Better.BuildNotification.Platform.MessageData.Models;
using Better.BuildNotification.Platform.Services;

namespace Better.BuildNotification.Platform.Tooling
{
    public class BuildPostPostProcessor
    {
        private const string NotSendingCloudNotification = "Not sending cloud notification";
        private const string SendingCloudNotification = "Sending cloud notification";

        private List<Error> errorList = new List<Error>();

        public void Clear()
        {
            errorList = new List<Error>();
        }

        public void AnalyzeSummary(BufferSummary bufferSummary)
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

            Console.WriteLine(reportLog);
        }
        
        private async void SendCloudMessage(BufferSummary summary)
        {
            if (!await FirebaseData.ValidateToken())
            {
                return;
            }
            
            var cloudMessagingData = FirebaseData.GetCloudMessagingData();
            var realtimeDatabaseData = FirebaseData.GetFirebaseAdminSDKData();
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

        public void UpdateSummary(ref BufferSummary summary)
        {
            summary.SetEndedAt(DateTime.UtcNow);
            summary.SetErrors(errorList);
            summary.UpdateBuildSize();
        }

        public void AddError(Error error)
        {
            errorList.Add(error);
        }
    }
}