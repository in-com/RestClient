using Incom.Web.HubClient.Models;
using Incom.Web.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace Incom.Web.HubClient
{
    public class HubClient
    {
        #region Fields

        /// <summary>
        /// Enthält die Optionen für den <see cref="HubClient"/>.
        /// </summary>
        internal HubClientOptions _options;

        /// <summary>
        /// Enthält die Instanz des <see cref="AuthenticationProvider{TOptions}"/>.
        /// </summary>
        internal AuthenticationProvider<HubClientOptions> _authProvider;

        /// <summary>
        /// Enthält die Instanz des <see cref="HubProvider{TOptions}"/>.
        /// </summary>
        private WinFriedHubProvider _hubProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gibt die Optionen des Clients.
        /// </summary>
        public HubClientOptions Options {
            get
            {
                return _options;
            }
        }

        /// <summary>
        /// Gibt einen Wert der Angibt ob die Authorisierung am API-Server erfolgreich war.
        /// </summary>
        public bool IsValidated
        {
            get
            {
                return _authProvider.IsValidated;
            }
        }

        /// <summary>
        /// Gibt einen Wert der Angibt ob eine Verbindung zum Hub besteht.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _hubProvider.IsConnected;
            }
        }

        /// <summary>
        /// Gibt die ConnectionId der momentanen Verbindung.
        /// </summary>
        public string ConnectionId
        {
            get
            {
                return _hubProvider.ConnectionId;
            }
        }

        /// <summary>
        /// Gibt den <see cref="AuthenticationContext"/> der Verbindung.
        /// </summary>
        public AuthenticationContext Context
        {
            get
            {
                return _authProvider.Context;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="HubClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public HubClient(HubClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            ThrowIfInvalidOptions(_options);
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="HubClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public HubClient(Action<HubClientOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = new HubClientOptions();
            options(_options);
            ThrowIfInvalidOptions(_options);
        }

        #endregion

        #region Methods (Public)

        /// <summary>
        /// Stellt eine Verbindung zum API-Server her.
        /// </summary>
        /// <remarks>Das Token/ID Token wird hier ermittelt.</remarks>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            if (_authProvider == null)
                _authProvider = new HubCredentialsAuthenticationProvider(_options);

            if (_hubProvider == null)
                _hubProvider = new WinFriedHubProvider(_options);

            // Am API-Server authentifizieren.
            await _authProvider.AuthorizeAsync();

            // Mit Hub Verbinden.
            await _hubProvider.ConnectAsync(Context.Cookies);
        }

        /// <summary>
        /// Trennt die Verbindung zum Hub.
        /// </summary>
        public void Disconnect()
        {
            _hubProvider.Disconnect();
        }

        /// <summary>
        /// Setzt eine neue API Serveradresse.
        /// </summary>
        /// <param name="serverAddress">Die neue Adresse.</param>
        public void SetServerAddress(string serverAddress)
        {
            if (string.IsNullOrWhiteSpace(serverAddress))
                throw new ArgumentNullException(nameof(serverAddress));

            _options.ServerAddress = serverAddress;
        }

        /// <summary>
        /// Setzt neue <see cref="ClientCredentials"/>.
        /// </summary>
        /// <param name="clientId">Die neue ClientId.</param>
        /// <param name="clientSecret">Das neue Client Secret.</param>
        public void SetClientCredentials(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException(nameof(clientId));

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException(nameof(clientSecret));

            _options.Credentials = new ClientCredentials()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
        }

        /// <summary>
        /// Wenn ausgeführt, sendet der Server seine Version zum ausführenden Client zurück.
        /// </summary>
        /// <returns></returns>
        public async Task GetApiVersion()
        {
            await _hubProvider.Hub.Invoke("ApiVersion");
        }

        /// <summary>
        /// Sendet Grabdaten an einen anderen Client.
        /// </summary>
        /// <param name="connectionId">Die ConnectionId eines anderen Clients im Hub.</param>
        /// <param name="graveId">Die ID des Grabes.</param>
        /// <returns></returns>
        public async Task SendGraveInformation(string connectionId, int graveId)
        {
            await _hubProvider.Hub.Invoke("SendSingleGraveInformation", connectionId, graveId);
        }

        /// <summary>
        /// Sendet Grabdaten an einen anderen Client.
        /// </summary>
        /// <param name="connectionId">Die ConnectionId eines anderen Clients im Hub.</param>
        /// <param name="graveIds">Die IDs von unterschiedlichen Gräbern.</param>
        /// <returns></returns>
        public async Task SendGraveInformation(string connectionId, List<int> graveIds)
        {
            await _hubProvider.Hub.Invoke("SendMultipleGraveInformations", connectionId, graveIds);
        }

        /// <summary>
        /// Sendet Grabdaten an einen anderen Client.
        /// </summary>
        /// <param name="connectionId">Die ConnectionId eines anderen Clients im Hub.</param>
        /// <param name="cementery">Die Nummer des Friedhofs.</param>
        /// <param name="graveNr">Die Nummer des Grabs.</param>
        /// <returns></returns>
        public async Task SendGraveInformation(string connectionId, string cementery, string graveNr)
        {
            await _hubProvider.Hub.Invoke("SendSingleGraveInformation", connectionId, cementery, graveNr);
        }

        /// <summary>
        /// Sendet Grabdaten an einen anderen Client.
        /// </summary>
        /// <param name="connectionId">Die ConnectionId eines anderen Clients im Hub.</param>
        /// <param name="cementery">Die Nummer des Friedhofs.</param>
        /// <param name="graveNrs">Die Nummern von unterschiedlichen Gräbern.</param>
        /// <returns></returns>
        public async Task SendGraveInformation(string connectionId, string cementery, List<string> graveNrs)
        {
            await _hubProvider.Hub.Invoke("SendMultipleGraveInformations", connectionId, cementery, graveNrs);
        }

        /// <summary>
        /// Sendet Grabdaten an einen anderen Client.
        /// </summary>
        /// <param name="connectionId">Die ConnectionId eines anderen Clients im Hub.</param>
        /// <param name="graves">Daten von Gräbern die gesendet werden sollen.</param>
        /// <returns></returns>
        [Obsolete("Diese Funktion wird in Zukunft nicht mehr unterstützt. Verwenden Sie stattdessen \"SendGraveInformation\"")]
        public async Task SendGraveData(string connectionId, List<Grab> graves)
        {
            await _hubProvider.Hub.Invoke("SendGraveData", connectionId, graves);
        }

        #endregion

        #region Methods (Private)

        /// <summary>
        /// Prüft die Eigenschaften der <see cref="HubClientOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        private void ThrowIfInvalidOptions(HubClientOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ServerAddress))
                throw new ArgumentNullException(nameof(HubClientOptions.ServerAddress));

            if (options.Credentials == null)
                throw new ArgumentNullException(nameof(HubClientOptions.Credentials));
            else if (!options.Credentials.HasValue)
                throw new ArgumentException(nameof(HubClientOptions.Credentials));

            if (string.IsNullOrWhiteSpace(options.Name))
                throw new ArgumentNullException(nameof(HubClientOptions.Name));
        }

        #endregion
    }
}
