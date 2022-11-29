using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Better.BuildNotification.Runtime.Tooling.Authorization
{
    [Serializable]
    public class FirebaseAdminSDKData
    {
        [SerializeField] private string type;
        [SerializeField] private string projectID;
        [SerializeField] private string privateKeyID;
        [SerializeField] private string privateKey;
        [SerializeField] private string clientEmail;
        [SerializeField] private string clientID;
        [SerializeField] private string authUri;
        [SerializeField] private string tokenUri;
        [SerializeField] private string authProviderX509CertURL;
        [SerializeField] private string clientX509CertURL;

        [JsonConstructor]
        public FirebaseAdminSDKData(string type, string projectID, string privateKeyID, string privateKey,
            string clientEmail,
            string clientID, string authUri, string tokenUri, string authProviderX509CertURL, string clientX509CertURL)
        {
            this.type = type;
            this.projectID = projectID;
            this.privateKeyID = privateKeyID;
            this.privateKey = privateKey;
            this.clientEmail = clientEmail;
            this.clientID = clientID;
            this.authUri = authUri;
            this.tokenUri = tokenUri;
            this.authProviderX509CertURL = authProviderX509CertURL;
            this.clientX509CertURL = clientX509CertURL;
        }
        
        private FirebaseAdminSDKData(FirebaseAdminSDKData accountData)
        {
            type = accountData.type;
            projectID = accountData.projectID;
            privateKeyID = accountData.privateKeyID;
            clientEmail = accountData.clientEmail;
            clientID = accountData.clientID;
            authUri = accountData.authUri;
            tokenUri =accountData.tokenUri;
            authProviderX509CertURL = accountData.authProviderX509CertURL;
            clientX509CertURL = accountData.clientX509CertURL;
        }

        [JsonProperty("type")] public string Type => type;

        [JsonProperty("project_id")] public string ProjectID => projectID;

        [JsonProperty("private_key_id")] public string PrivateKeyID => privateKeyID;

        [JsonProperty("private_key")] public string PrivateKey => privateKey;

        [JsonProperty("client_email")] public string ClientEmail => clientEmail;

        [JsonProperty("client_id")] public string ClientID => clientID;

        [JsonProperty("auth_uri")] public string AuthUri => authUri;

        [JsonProperty("token_uri")] public string TokenUri => tokenUri;

        [JsonProperty("auth_provider_x509_cert_url")]
        public string AuthProviderX509CertURL => authProviderX509CertURL;

        [JsonProperty("client_x509_cert_url")] public string ClientX509CertURL => clientX509CertURL;

        [JsonIgnore] public bool IsValid => !string.IsNullOrEmpty(projectID);
    }
}