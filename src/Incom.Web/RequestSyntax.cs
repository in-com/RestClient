using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Incom.Web.Models;

namespace Incom.Web
{
    public class RequestSyntax : IRequestSyntax
    {
        #region Fields

        /// <summary>
        /// Enthält die aufrufenden Client.
        /// </summary>
        internal IRestClient _client;

        /// <summary>
        /// Enthält den Client der für die Abfragen an den API-Server verwendet wird.
        /// </summary>
        private HttpClient _httpClient;

        #endregion

        #region Properties

        /// <summary>
        /// Gibt die API-Server Adresse.
        /// </summary>
        public Uri ServerAddress { get; private set; }

        /// <summary>
        /// Gibt den zu verwendenen Content-Type.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gibt den Body Inhalt.
        /// </summary>
        public StringContent Body { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="RequestSyntax"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public RequestSyntax(IRestClient client, ApiEndpointVersion? endpointVersion)
        {
            _client = client;

            if (endpointVersion.HasValue)
            {
                switch (endpointVersion.Value)
                {
                    case ApiEndpointVersion.Version1:
                        ServerAddress = new Uri(_client.Options.ServerAddress.TrimEnd('/') + "/api/v1");
                        break;
                    default:
                        ServerAddress =  new Uri(_client.Options.ServerAddress.TrimEnd('/') + "/api/v2");
                        break;
                }
            }
            else
            {
                ServerAddress = new Uri(_client.Options.ServerAddress.TrimEnd('/') + "/api");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fügt dem aktuellen Pfad ein neues Segment hinzu.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public IRequestSyntax AppendPathSegment(object segment, bool encode = true)
        {
            ServerAddress = ServerAddress.AppendSegment(segment.ToInvariantString(), encode);
            return this;
        }

        /// <summary>
        /// Fügt dem aktuellen Pfad ein Query String Parameter hinzu.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IRequestSyntax SetQueryParam(string name, object value)
        {
            ServerAddress = ServerAddress.SetQueryParam(name, value.ToInvariantString());
            return this;
        }

        /// <summary>
        /// Fügt dem aktuellen Pfad ein oder mehrere Query String Parameter hinzu.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public IRequestSyntax SetQueryParams(object queryParams)
        {
            var queries = GetQueryParams(queryParams);
            ServerAddress = ServerAddress.SetQueryParam(queries.ToDictionary(q => q.Key, q => q.Value));
            return this;
        }

        /// <summary>
        /// Fügt der Request Body-Daten hinzu.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public IRequestSyntax SetBody(object body, string contentType = "application/json")
        {
            ContentType = contentType;

            if (body != null)
                Body = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, contentType);

            return this;
        }

        /// <summary>
        /// Führt eine GET-Anfrage auf dem Server aus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetAsync<T>()
        {
            return (T)await GetAsync(typeof(T));
        }

        /// <summary>
        /// Führt eine GET-Anfrage auf dem Server aus.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(Type type)
        {
            HttpClient client = await GetHttpClientAsync();
            Uri apiEndpoint = ServerAddress;

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiEndpoint);
                return await response.GetContentAsync(type);
            }
            catch (Exception ex)
            {
                var context = new EndpointFailedContext(apiEndpoint, ex);
                await RunEndpointEventsAsync(context);

                if (!context.IsHandled)
                    throw context.Error;
            }

            return null;
        }

        /// <summary>
        /// Führt eine POST-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        public async Task PostAsync()
        {
            HttpClient client = await GetHttpClientAsync();
            Uri apiEndpoint = ServerAddress;

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiEndpoint, Body);
                await response.GetContentAsync();
            }
            catch (Exception ex)
            {
                var context = new EndpointFailedContext(apiEndpoint, ex);
                await RunEndpointEventsAsync(context);

                if (!context.IsHandled)
                    throw context.Error;
            }
        }

        /// <summary>
        /// Führt eine POST-Anfrage auf dem Server aus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>()
        {
            return (T)await PostAsync(typeof(T));
        }

        /// <summary>
        /// Führt eine POST-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        public async Task<object> PostAsync(Type type)
        {
            HttpClient client = await GetHttpClientAsync();
            Uri apiEndpoint = ServerAddress;

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiEndpoint, Body);
                await response.GetContentAsync(type);
            }
            catch (Exception ex)
            {
                var context = new EndpointFailedContext(apiEndpoint, ex);
                await RunEndpointEventsAsync(context);

                if (!context.IsHandled)
                    throw context.Error;
            }

            return null;
        }

        /// <summary>
        /// Führt eine PUT-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        public async Task PutAsync()
        {
            HttpClient client = await GetHttpClientAsync();
            Uri apiEndpoint = ServerAddress;

            try
            {
                HttpResponseMessage response = await client.PutAsync(apiEndpoint, Body);
                await response.GetContentAsync();
            }
            catch (Exception ex)
            {
                var context = new EndpointFailedContext(apiEndpoint, ex);
                await RunEndpointEventsAsync(context);

                if (!context.IsHandled)
                    throw context.Error;
            }
        }

        /// <summary>
        /// Führt eine DELETE-Anfrage auf dem Server aus.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            HttpClient client = await GetHttpClientAsync();
            Uri apiEndpoint = ServerAddress;

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(apiEndpoint);
                await response.GetContentAsync();
            }
            catch (Exception ex)
            {
                var context = new EndpointFailedContext(apiEndpoint, ex);
                await RunEndpointEventsAsync(context);

                if (!context.IsHandled)
                    throw context.Error;
            }
        }

        #endregion

        #region Methods (Private)

        /// <summary>
        /// Ermittelt die Eigenschaften samt deren Werten aus einem dynamischen objekt.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, string>> GetQueryParams(object obj)
        {
            return from prop in obj.GetType().GetRuntimeProperties()
                   let val = prop.GetValue(obj, null)
                   select new KeyValuePair<string, string>(prop.Name, val.ToInvariantString());
        }

        /// <summary>
        /// Wenn <see cref="EndpointFailedContext"/> angegeben sind, dann ausführen.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task RunEndpointEventsAsync(EndpointFailedContext context)
        {
            if (_client.Options.EndpointEvents?.OnRequestFailed != null)
                await _client.Options.EndpointEvents.OnRequestFailed(context);

            if (!context.IsHandled)
                throw context.Error;
        }

        /// <summary>
        /// Gibt einen <see cref="HttpClient"/> fertig konfiguriert für API-Abfragen zurück.
        /// </summary>
        /// <returns></returns>
        private async Task<HttpClient> GetHttpClientAsync()
        {
            // Token aus dem DataStore laden.
            Token token = await _client.Options.DataStore.GetAsync(_client.Options.Credentials.GetCredentials().Key);

            // Prüfen ob ein Token aus dem DataStore geladen werden konnte. Wenn ja muss geprüft
            // werden ob dieser noch gültig ist. Wenn das Token gültig ist, wird es zurückgegeben.
            // Falls es nicht gültig ist, wird versucht ein neues anzufordern.
            if (token != null)
            {
                try
                {
                    string payload = Jose.JWT.Payload(_client.Context.AccessToken.AccessToken);
                    JwtToken jwtToken = await Task.Run(() => JsonConvert.DeserializeObject<JwtToken>(payload));
                    DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(jwtToken.ExpirationTime);

                    if (date.UtcDateTime < DateTime.UtcNow)
                        token = null;
                }
                catch
                {
                    if (token.Issued.Value.AddSeconds(token.ExpiresIn) < DateTime.Now)
                        token = null;
                }
            }

            // Falls kein Token vorhanden, neues anfordern.
            if (token == null)
            {
                await _client.ConnectAsync();
                if (_client.IsValidated)
                    token = _client.Context.AccessToken;
                else
                    return await Task.FromResult<HttpClient>(null);
            }

            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                return _httpClient;
            }
            else
            {
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip
                };

                _httpClient = new HttpClient(handler);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

                return _httpClient;
            }
        }

        #endregion
    }
}
