using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BuildNotification.Runtime.Tooling.DatabaseModule;
using BuildNotification.Runtime.Tooling.DatabaseModule.RequestWrappers;
using Jose;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace BuildNotification.Runtime.Tooling.Authorization
{
    public class FirebaseCustomToken
    {
        public const double ExpirationSeconds = 3600;
        private readonly FirebaseAdminSDKData _serviceAccountData;

        public FirebaseCustomToken(FirebaseAdminSDKData serviceAccountData)
        {
            _serviceAccountData = serviceAccountData;
        }

        public static long GetExpirationTime(DateTimeOffset now)
        {
            return now.AddSeconds(ExpirationSeconds).ToUnixTimeSeconds();
        }

        public static DateTimeOffset FromSecond(long seconds)
        {
            return new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero).AddSeconds(
                seconds);
        }

        public static async Task<(TokenResponse, TokenResponseError)> MakeTokenRequest(FirebaseAdminSDKData service,
            string[] scores,
            DateTimeOffset now)
        {
            var token = new FirebaseCustomToken(service);

            var assertion = token.CreateToken(now, scores);

            var request = new TokenRequest("urn:ietf:params:oauth:grant-type:jwt-bearer", service.TokenUri);
            request.SetToken(assertion);
            var database = new SingleDatabase(request);
            var (tokenResponse, responseError) =
                await database.PostAsync<TokenRequest, TokenResponse, TokenResponseError>(
                    new SingleWrapper<TokenRequest>(request));

            if (responseError is SingleWrapper<TokenResponseError> error) return (null, error.Data);

            if (tokenResponse is SingleWrapper<TokenResponse> response) return (response.Data, null);

            throw new HttpRequestException("Response has wrong type");
        }

        public string CreateToken(DateTimeOffset now, string[] scores)
        {
            var nowSeconds = now.ToUnixTimeSeconds();
            var inOneHour = GetExpirationTime(now);

            var scope = string.Join(" ", scores);

            var payload = new Dictionary<string, object>
            {
                { "iss", _serviceAccountData.ClientEmail },
                { "sub", _serviceAccountData.ClientEmail },
                { "scope", scope },
                { "aud", _serviceAccountData.TokenUri },
                { "iat", nowSeconds },
                { "exp", inOneHour }
            };

            return SignToken(payload);
        }

        private string SignToken(Dictionary<string, object> payload)
        {
            string jwt;
            RsaPrivateCrtKeyParameters key;
            using (var stringReader = new StringReader(_serviceAccountData.PrivateKey))
            {
                var pemReader = new PemReader(stringReader);
                key = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();
            }

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(ToRsaParameters(key));
                jwt = JWT.Encode(payload, rsa, JwsAlgorithm.RS256, GenerateExtraHeaders());
            }

            return jwt;
        }

        private Dictionary<string, object> GenerateExtraHeaders()
        {
            return new Dictionary<string, object>()
            {
                { "kid", _serviceAccountData.PrivateKeyID }
            };
        }

        /// <summary>
        /// https://github.com/neoeinstein/bouncycastle/blob/master/crypto/src/security/DotNetUtilities.cs
        /// </summary>
        /// <param name="privKey">string</param>
        /// <returns></returns>
        private static RSAParameters ToRsaParameters(RsaPrivateCrtKeyParameters privKey) => new RSAParameters
        {
            Modulus = privKey.Modulus.ToByteArrayUnsigned(),
            Exponent = privKey.PublicExponent.ToByteArrayUnsigned(),
            D = privKey.Exponent.ToByteArrayUnsigned(),
            P = privKey.P.ToByteArrayUnsigned(),
            Q = privKey.Q.ToByteArrayUnsigned(),
            DP = privKey.DP.ToByteArrayUnsigned(),
            DQ = privKey.DQ.ToByteArrayUnsigned(),
            InverseQ = privKey.QInv.ToByteArrayUnsigned()
        };
    }
}