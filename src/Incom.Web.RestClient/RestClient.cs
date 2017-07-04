using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    public class RestClient
    {
        #region Fields

        /// <summary>
        /// Enthält die Optionen für den <see cref="RestClient"/>.
        /// </summary>
        internal RestClientOptions _options;

        /// <summary>
        /// Enthält die Instanz des <see cref="AuthenticationProvider"/>.
        /// </summary>
        internal AuthenticationProvider _authProvider;

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
                switch (_options.RestApiType)
                {
                    case RestApiType.WinFriedSE:
                    case RestApiType.Terminplaner:
                        _authProvider = new ClientCredentialsAuthenticationProvider(_options);
                        break;
                }
            }

            // Am API-Server authentifizieren.
            await _authProvider.AuthorizeAsync();

            // Authorization Events ausführen.
            await RunAuthenticationEventsAsync();
        }

        /// <summary>
        /// Sendet eine Anfrage an den API-Server.
        /// </summary>
        /// <returns></returns>
        public IRequestSyntax Request()
        {
            return new RequestSyntax(this);
        }

        #endregion

        #region Methods: Private

        /// <summary>
        /// Prüft die Eigenschaften der <see cref="RestClientOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        private void ThrowIfInvalidOptions(RestClientOptions options)
        {
            if (string.IsNullOrEmpty(options.ServerAddress))
                throw new ArgumentNullException(nameof(RestClientOptions.ServerAddress));

            if (options.Credentials == null)
                throw new ArgumentNullException(nameof(RestClientOptions.Credentials));
            else if (!options.Credentials.HasValue)
                throw new ArgumentException(nameof(RestClientOptions.Credentials));

            if (options.RestApiType == RestApiType.Terminplaner && options.SigningKey == null)
                throw new ArgumentNullException("Wenn eine Verbindung mit der Terminplaner API hergestellt werden soll, muss ein SigningKey angegeben werden.", nameof(RestClientOptions.SigningKey));
        }

        /// <summary>
        /// Wenn <see cref="AuthenticationEvents"/> angegeben sind, dann ausführen.
        /// </summary>
        /// <returns></returns>
        private async Task RunAuthenticationEventsAsync()
        {
            if (_authProvider.IsValidated)
            {
                await _options.DataStore.StoreAsync(_options.Credentials.GetCredentials().Key, _authProvider.AccessToken);
                if (_options.AuthenticationEvents?.OnAuthorized != null)
                    await _options.AuthenticationEvents.OnAuthorized(_authProvider.AccessToken);
            }
            else
            {
                var context = new AuthenticationFailedContext(_options.ServerAddress, _authProvider.Error);
                if (_options.AuthenticationEvents?.OnAuthorizationFailed != null)
                    await _options.AuthenticationEvents.OnAuthorizationFailed(context);

                if (!context.IsHandled)
                    throw _authProvider.Error;
            }
        }

        #endregion
    }
}
