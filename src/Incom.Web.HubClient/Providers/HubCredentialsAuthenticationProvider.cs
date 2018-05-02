using Incom.Web.HubClient.Models;
using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.HubClient
{
    public class HubCredentialsAuthenticationProvider : AuthenticationProvider<HubClientOptions>
    {
        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="HubCredentialsAuthenticationProvider"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public HubCredentialsAuthenticationProvider(HubClientOptions options)
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
                            new KeyValuePair<string, string>("grant_type", "hub_credentials"),
                        };
            var content = new FormUrlEncodedContent(pairs);

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = cookies
            };

            // Die Anfrage an den Server senden.
            using (var client = new HttpClient(handler))
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GenerateCredentials(credentials));

                    Uri tokenEndpoint = new Uri(new Uri(Options.ServerAddress), "oauth2/token");
                    var response = await client.PostAsync(Path.Combine(tokenEndpoint.AbsoluteUri), content);
                    var result = await response.Content.ReadAsStringAsync();
                    var token = await response.GetContentAsync<Token>();
                    var responseCookies = await response.GetCookiesAsync(cookies);
                    Validated(token, responseCookies);
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
                finally
                {
                    await RunAuthenticationEventsAsync();
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

        /// <summary>
        /// Wenn <see cref="AuthenticationEvents"/> angegeben sind, dann ausführen.
        /// </summary>
        /// <returns></returns>
        private async Task RunAuthenticationEventsAsync()
        {
            if (IsValidated)
            {
                if (Options.AuthenticationEvents?.OnAuthorized != null)
                    await Options.AuthenticationEvents.OnAuthorized(Context);
            }
            else
            {
                var context = new AuthenticationFailedContext(Options.ServerAddress, Error);
                if (Options.AuthenticationEvents?.OnAuthorizationFailed != null)
                    await Options.AuthenticationEvents.OnAuthorizationFailed(context);

                if (!context.IsHandled)
                    throw Error;
            }
        }

        #endregion
    }
}