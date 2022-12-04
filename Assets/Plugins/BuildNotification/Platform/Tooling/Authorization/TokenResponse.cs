using System;
using Newtonsoft.Json;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class TokenResponse
    {
        [JsonConstructor]
        public TokenResponse(string defaultToken, string idToken, string scope, string tokenType, int expiresIn)
        {
            DefaultToken = defaultToken;
            IDToken = idToken;
            Scope = scope;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
        }

        [JsonIgnore] public string AccessToken => string.IsNullOrEmpty(DefaultToken) ? IDToken : DefaultToken;

        [JsonProperty("access_token")] private string DefaultToken { get; set; }
        [JsonProperty("id_token")] private string IDToken { get; set; }
        [JsonProperty("scope")] public string Scope { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
    }
}