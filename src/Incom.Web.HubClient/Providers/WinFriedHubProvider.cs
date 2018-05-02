using Incom.Web.HubClient.Models;
using Incom.Web.Models;
using Microsoft.AspNet.SignalR.Client;
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
    public class WinFriedHubProvider : HubProvider<HubClientOptions>
    {
        #region Fields

        ///// <summary>
        ///// Endpunkt zum SignalR Server.
        ///// </summary>
        //const string SIGNALR_PREFIX = "hub";

        /// <summary>
        /// Name des WinFried Hubs.
        /// </summary>
        const string HUB_PREFIX = "WinFriedHub";

        /// <summary>
        /// Enthält die momentane Verbindung zum Hub.
        /// </summary>
        private HubConnection _connection;

        #endregion

        #region Properties

        /// <summary>
        /// Gibt die momentane ConnectionId.
        /// </summary>
        public string ConnectionId
        {
            get
            {
                return _connection?.ConnectionId;
            }
        }

        /// <summary>
        /// Gibt den HubProxy.
        /// </summary>
        public IHubProxy Hub { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="WinFriedHubProvider"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public WinFriedHubProvider(HubClientOptions options)
            : base(options)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Methode für das herstellen einer Verbindung zum Hub.
        /// </summary>
        /// <param name="cookies">Cookies die zur Authentifizierung am Hub verwendet werden.</param>
        public override async Task ConnectAsync(IEnumerable<Cookie> cookies)
        {
            try
            {
                var queryString = new Dictionary<string, string>()
                {
                    { "name", Options.Name }
                };

                _connection = new HubConnection(Options.ServerAddress, queryString)
                {
                    CookieContainer = new CookieContainer()
                };

                foreach (Cookie cookie in cookies)
                {
                    _connection.CookieContainer.Add(cookie);
                }

                Hub = _connection.CreateHubProxy(HUB_PREFIX);

                if (Options.HubEvents?.OnConnected != null)
                    Hub.On(nameof(Options.HubEvents.OnConnected), async () => await Options.HubEvents.OnConnected());

                if (Options.HubEvents?.OnClientConnected != null)
                    Hub.On<Client>(nameof(Options.HubEvents.OnClientConnected), async (client) => await Options.HubEvents.OnClientConnected(client));

                if (Options.HubEvents?.OnClientDisconnected != null)
                    Hub.On<Client>(nameof(Options.HubEvents.OnClientDisconnected), async (client) => await Options.HubEvents.OnClientDisconnected(client));

                if (Options.HubEvents?.OnApiVersion != null)
                    Hub.On<string>(nameof(Options.HubEvents.OnApiVersion), async (version) => await Options.HubEvents.OnApiVersion(version));

                if (Options.HubEvents?.OnMessage != null)
                    Hub.On<string>(nameof(Options.HubEvents.OnMessage), async (message) => await Options.HubEvents.OnMessage(message));

                if (Options.HubEvents?.OnGraveDataReceived != null)
                    Hub.On<IEnumerable<Grab>>(nameof(Options.HubEvents.OnGraveDataReceived), async (gräber) => await Options.HubEvents.OnGraveDataReceived(gräber));

                if (Options.HubEvents?.OnGraveCreated != null)
                    Hub.On<Grab>(nameof(Options.HubEvents.OnGraveCreated), async (grab) => await Options.HubEvents.OnGraveCreated(grab));

                if (Options.HubEvents?.OnGraveChanged != null)
                    Hub.On<Grab>(nameof(Options.HubEvents.OnGraveChanged), async (grab) => await Options.HubEvents.OnGraveChanged(grab));

                if (Options.HubEvents?.OnGraveArchived != null)
                    Hub.On<Grab, Grab>(nameof(Options.HubEvents.OnGraveArchived), async (archived, created) => await Options.HubEvents.OnGraveArchived(archived, created));

                if (Options.HubEvents?.OnGraveSetActive != null)
                    Hub.On<Grab>(nameof(Options.HubEvents.OnGraveSetActive), async (grab) => await Options.HubEvents.OnGraveSetActive(grab));

                if (Options.HubEvents?.OnGraveDeleted != null)
                    Hub.On<Grab>(nameof(Options.HubEvents.OnGraveDeleted), async (grab) => await Options.HubEvents.OnGraveDeleted(grab));

                if (Options.HubEvents?.OnGraveJoined != null)
                    Hub.On<Grab, Grab>(nameof(Options.HubEvents.OnGraveJoined), async (joined, deleted) => await Options.HubEvents.OnGraveJoined(joined, deleted));

                if (Options.HubEvents?.OnGraveDetached != null)
                    Hub.On<Grab, Grab>(nameof(Options.HubEvents.OnGraveDetached), async (old, created) => await Options.HubEvents.OnGraveDetached(old, created));

                if (Options.HubEvents?.OnGraveRightOfUseEnded != null)
                    Hub.On<Grab>(nameof(Options.HubEvents.OnGraveRightOfUseEnded), async (grab) => await Options.HubEvents.OnGraveRightOfUseEnded(grab));

                _connection.Closed += Connection_Closed;
                _connection.Error += Connection_Error;

                _connection.TraceLevel = Options.TraceLevel;
                if (Options.TraceWriter != null)
                    _connection.TraceWriter = Options.TraceWriter;

                await _connection.Start();
                Connected();
            }
            catch (HttpClientException clientEx)
            {
                Rejected(clientEx.Response.StatusCode, clientEx.Message);
            }
        }
        
        /// <summary>
        /// Schließt eine vorhandene Verbindung.
        /// </summary>
        public override void Disconnect()
        {
            if (_connection?.State == ConnectionState.Connected)
            {
                _connection.Stop();
                Disconnected();
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein Fehler in der Verbindung auftritt.
        /// </summary>
        /// <param name="obj">Der Fehler als <see cref="Exception"/>.</param>
        private void Connection_Error(Exception obj)
        {
            Disconnected();

            if (Options.HubEvents?.OnError != null)
                Options.HubEvents.OnError(obj);
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Verbindung zum Hub getrennt wird.
        /// </summary>
        private async void Connection_Closed()
        {
            Disconnected();

            if (Options.HubEvents?.OnDisconnected != null)
                await Options.HubEvents.OnDisconnected();
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