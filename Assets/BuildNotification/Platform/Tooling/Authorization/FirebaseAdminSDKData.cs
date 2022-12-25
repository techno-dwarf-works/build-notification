using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class FirebaseAdminSDKData
    {
        [JsonProperty("type")] private string _type;
        [JsonProperty("project_id")] private string _projectID;
        [JsonProperty("private_key_id")] private string _privateKeyID;
        [JsonProperty("private_key")] private string _privateKey;
        [JsonProperty("client_email")] private string _clientEmail;
        [JsonProperty("client_id")] private string _clientID;
        [JsonProperty("auth_uri")] private string _authUri;
        [JsonProperty("token_uri")] private string _tokenUri;
        [JsonProperty("auth_provider_x509_cert_url")] private string _authProviderX509CertURL;
        [JsonProperty("client_x509_cert_url")] private string _clientX509CertURL;

        [JsonConstructor]
        public FirebaseAdminSDKData([JsonProperty("type")] string type,
            [JsonProperty("project_id")] string projectID,
            [JsonProperty("private_key_id")] string privateKeyID,
            [JsonProperty("private_key")] string privateKey,
            [JsonProperty("client_email")] string clientEmail,
            [JsonProperty("client_id")] string clientID,
            [JsonProperty("auth_uri")] string authUri,
            [JsonProperty("token_uri")] string tokenUri,
            [JsonProperty("auth_provider_x509_cert_url")]
            string authProviderX509CertURL,
            [JsonProperty("client_x509_cert_url")] string clientX509CertURL)
        {
            _type = type;
            _projectID = projectID;
            _privateKeyID = privateKeyID;
            _privateKey = privateKey;
            _clientEmail = clientEmail;
            _clientID = clientID;
            _authUri = authUri;
            _tokenUri = tokenUri;
            _authProviderX509CertURL = authProviderX509CertURL;
            _clientX509CertURL = clientX509CertURL;
        }

        public FirebaseAdminSDKData()
        {
            _type = string.Empty;
            _projectID = string.Empty;
            _privateKeyID = string.Empty;
            _privateKey = string.Empty;
            _clientEmail = string.Empty;
            _clientID = string.Empty;
            _authUri = string.Empty;
            _tokenUri = string.Empty;
            _authProviderX509CertURL = string.Empty;
            _clientX509CertURL = string.Empty;
        }

        private FirebaseAdminSDKData(FirebaseAdminSDKData accountData)
        {
            _type = accountData._type;
            _projectID = accountData._projectID;
            _privateKeyID = accountData._privateKeyID;
            _clientEmail = accountData._clientEmail;
            _clientID = accountData._clientID;
            _authUri = accountData._authUri;
            _tokenUri = accountData._tokenUri;
            _authProviderX509CertURL = accountData._authProviderX509CertURL;
            _clientX509CertURL = accountData._clientX509CertURL;
        }

        public string Type => _type;
        public string ProjectID => _projectID;
        public string PrivateKeyID => _privateKeyID;
        public string PrivateKey => _privateKey;
        public string ClientEmail => _clientEmail;
        public string ClientID => _clientID;
        public string AuthUri => _authUri;
        public string TokenUri => _tokenUri;

        public string AuthProviderX509CertURL => _authProviderX509CertURL;
        public string ClientX509CertURL => _clientX509CertURL;

        [JsonIgnore] public bool IsValid => !string.IsNullOrEmpty(_projectID);
    }
}