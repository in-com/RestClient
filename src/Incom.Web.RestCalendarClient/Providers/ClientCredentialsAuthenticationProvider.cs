﻿using Incom.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestCalendarClient
{
    public class ClientCredentialsAuthenticationProvider : AuthenticationProvider<RestCalendarClientOptions>
    {
        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ClientCredentialsAuthenticationProvider"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public ClientCredentialsAuthenticationProvider(RestCalendarClientOptions options)
            : base(options)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Methode zur Authorisierung an einem API-Server.
        /// </summary>
        public override async Task AuthorizeAsync()
        {
            // Zugangsdaten ermitteln.
            KeyValuePair<string, string> credentials = Options.Credentials.GetCredentials();

            // Parameter für die Anfrage.
            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        };
            var content = new FormUrlEncodedContent(pairs);

            // Die Anfrage an den Server senden.
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GenerateCredentials(credentials));

                    Uri tokenEndpoint = new Uri(new Uri(Options.ServerAddress), "oauth2/token");
                    var response = await client.PostAsync(Path.Combine(tokenEndpoint.AbsoluteUri), content);
                    var token = await response.GetContentAsync<Token>();
                    Jose.JWT.Decode(token.AccessToken, Options.SigningKey.GetRSAPublicKey(), Jose.JwsAlgorithm.RS256);
                    Validated(token);
                }
                catch (RestApiException apiEx)
                {
                    if (apiEx.Message.Contains("<title>401 - "))
                        Rejected(apiEx.StatusCode, "Access is denied due to invalid credentials. \n\nMake sure that \"Basic Authentication\" is disabled on the IIS Server.");
                    else
                        Rejected(apiEx.StatusCode, apiEx.Message);
                }
                catch (Exception ex)
                {
                    Rejected(0, ex.Message);
                }
            }
        }

        /// <summary>
        /// Generiert ein CredentialSet für die Authentifizierung bei der WebAPI.
        /// </summary>
        /// <returns></returns>
        private string GenerateCredentials(KeyValuePair<string, string> credentials)
        {
            string clientid = credentials.Key;
            string clientSecret = credentials.Value;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(clientid + ":" + clientSecret));
        }

        #endregion
    }
}
