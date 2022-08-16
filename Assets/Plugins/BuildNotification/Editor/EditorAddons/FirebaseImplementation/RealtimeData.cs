using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BetterAttributes.Runtime.EditorAddons.ReadOnlyAttributes;
using BuildNotification.EditorAddons.Interfaces;
using BuildNotification.Runtime;
using UnityEngine;

namespace BuildNotification.EditorAddons.FirebaseImplementation
{
    [Serializable]
    public class RealtimeDatabaseData : ISendData
    {
        [TextArea(2, 5)] [SerializeField] private string urlBase = "https://{0}-default-rtdb.firebaseio.com/";

        [ReadOnlyField] [TextArea(2, 5)] [SerializeField]
        private string[] scope = new string[]
            { "https://www.googleapis.com/auth/firebase.database", "https://www.googleapis.com/auth/userinfo.email" };

        [ReadOnlyField] [TextArea(2, 5)] [SerializeField]
        private string storedBaseUrl;

        [ReadOnlyField] [TextArea(10, 15)] [SerializeField]
        private string token;

        public string[] Scopes => scope;
        public string BaseUrl => storedBaseUrl;
        public string RequestUrl { get; }
        public string RequestBatchUrl { get; }

        public void SetProject(string project)
        {
            storedBaseUrl = new Uri(new Uri(string.Format(urlBase, project)),
                $"{FirebaseDefinition.MessagesRoot}.{PathService.DefaultExtension}").ToString();
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