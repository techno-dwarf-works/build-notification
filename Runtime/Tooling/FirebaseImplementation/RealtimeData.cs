﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Better.Attributes.Runtime.ReadOnly;
using BuildNotification.Runtime.Services;
using BuildNotification.Runtime.Tooling.Interfaces;
using UnityEngine;

namespace BuildNotification.Runtime.Tooling.FirebaseImplementation
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
        
        public string RequestUrl => $"/{FirebaseDefinition.MessagesRoot}";
        public string RequestBatchUrl { get; }

        public void SetProject(string project)
        {
            storedBaseUrl = new Uri(string.Format(urlBase, project)).ToString();
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