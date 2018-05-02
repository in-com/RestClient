using Incom.Web.Models;
using System;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    public class RestClient : IRestClient
    {
        #region Fields

        /// <summary>
        /// Enthält die Optionen für den <see cref="RestClient"/>.
        /// </summary>
        internal RestClientOptions _options;

        /// <summary>
        /// Enthält die Instanz des <see cref="AuthenticationProvider{TOptions}"/>.
        /// </summary>
        internal AuthenticationProvider<RestClientOptions> _authProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gibt die Optionen des Clients.
        /// </summary>
        public IRestClientOptions Options {
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
        /// Initialisiert eine neue Instanz der <see cref="RestClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public RestClient(RestClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            ThrowIfInvalidOptions(_options);
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="RestClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public RestClient(Action<RestClientOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = new RestClientOptions();
            options(_options);
            ThrowIfInvalidOptions(_options);
        }

        #endregion

        #region Methods: Public

        /// <summary>
        /// Stellt eine Verbindung zum API-Server her.
        /// </summary>
        /// <remarks>Das Token/ID Token wird hier ermittelt.</remarks>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            if (_authProvider == null)
            {
                _authProvider = new ClientCredentialsAuthenticationProvider(_options);
            }

            // Am API-Server authentifizieren.
            await _authProvider.AuthorizeAsync();
        }

        /// <summary>
        /// Sendet eine Anfrage an den API-Server.
        /// </summary>
        /// <returns></returns>
        public IRequestSyntax Request(ApiEndpointVersion endpointVersion = ApiEndpointVersion.Version2)
        {
            return new RequestSyntax(this, endpointVersion);
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

        #endregion

        #region Methods: Private

        /// <summary>
        /// Prüft die Eigenschaften der <see cref="RestClientOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        private void ThrowIfInvalidOptions(RestClientOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ServerAddress))
                throw new ArgumentNullException(nameof(RestClientOptions.ServerAddress));

            if (options.Credentials == null)
                throw new ArgumentNullException(nameof(RestClientOptions.Credentials));
            else if (!options.Credentials.HasValue)
                throw new ArgumentException(nameof(RestClientOptions.Credentials));
        }

        #endregion
    }
}
