using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using BetterAttributes.Runtime.EditorAddons.ReadOnlyAttributes;
using BuildNotification.EditorAddons.Interfaces;
using BuildNotification.EditorAddons.Models;
using UnityEngine;

namespace BuildNotification.EditorAddons.FirebaseImplementation
{
    [Serializable]
    public class CloudMessagingData : ISendData
    {
        [TextArea(2, 5)] [SerializeField] private string urlBase = "https://fcm.googleapis.com";

        [TextArea(2, 5)] [SerializeField] private string requestUrlBase = "/v1/projects/{0}/messages:send";
        [TextArea(2, 5)] [SerializeField] private string batchUrlBase = "/batch";

        [ReadOnlyField] [TextArea(2, 5)] [SerializeField]
        private string[] scope = new string[]{"https://www.googleapis.com/auth/firebase.messaging"};

        [ReadOnlyField] [TextArea(2, 5)] [SerializeField]
        private string storedRequestUrl;

        [ReadOnlyField] [TextArea(10, 15)] [SerializeField]
        private string token;

        [TextArea(5, 10)] [SerializeField] private List<string> receiver;

        public string BaseUrl => urlBase;

        public string RequestUrl => storedRequestUrl;

        public string RequestBatchUrl => batchUrlBase;

        public List<Receiver> Receivers => receiver.Select(x => new Receiver(x)).ToList();

        public string[] Scopes => scope;

        public void SetProject(string project)
        {
            storedRequestUrl = string.Format(requestUrlBase, project);
        }

        public string GetContentType()
        {
            return "application/json";
        }

        public void SetToken(string newToken)
        {
            token = newToken;
        }

        public string GenerateToken()
        {
            return token;
        }

        public void GenerateHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GenerateToken());
        }
    }
}