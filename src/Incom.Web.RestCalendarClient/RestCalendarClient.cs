using Incom.Web.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Incom.Web.RestCalendarClient
{
    public class RestCalendarClient : IRestClient
    {
        #region Fields

        /// <summary>
        /// Enthält die Optionen für den <see cref="RestCalendarClient"/>.
        /// </summary>
        internal RestCalendarClientOptions _options;

        /// <summary>
        /// Enthält die Instanz des <see cref="AuthenticationProvider{TOptions}"/>.
        /// </summary>
        internal AuthenticationProvider<RestCalendarClientOptions> _authProvider;

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
        /// Enthält das Access Token bei erfolgreicher Authorisierung.
        /// </summary>
        public Token AccessToken
        {
            get
            {
                return _authProvider.AccessToken;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="RestCalendarClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public RestCalendarClient(RestCalendarClientOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            ThrowIfInvalidOptions();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="RestCalendarClient"/> Klasse.
        /// </summary>
        /// <param name="options"></param>
        public RestCalendarClient(Action<RestCalendarClientOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = new RestCalendarClientOptions();
            options(_options);
            ThrowIfInvalidOptions();
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

            // Authorization Events ausführen.
            await RunAuthenticationEventsAsync();
        }

        /// <summary>
        /// Sendet eine Anfrage an den API-Server.
        /// </summary>
        /// <returns></returns>
        public IRequestSyntax Request(ApiEndpointVersion endpointVersion = ApiEndpointVersion.Version2)
        {
            return new RequestSyntax(this, endpointVersion);
        }

        #endregion

        #region Methods: Private

        /// <summary>
        /// Prüft die Eigenschaften der <see cref="RestClientOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        private void ThrowIfInvalidOptions()
        {
            if (string.IsNullOrEmpty(_options.ServerAddress))
                throw new ArgumentNullException(nameof(RestCalendarClientOptions.ServerAddress));

            if (_options.Credentials == null)
                throw new ArgumentNullException(nameof(RestCalendarClientOptions.Credentials));
            else if (!_options.Credentials.HasValue)
                throw new ArgumentException(nameof(RestCalendarClientOptions.Credentials));

            if (_options.SigningKey == null)
                throw new ArgumentNullException("Wenn eine Verbindung mit der Terminplaner API hergestellt werden soll, muss ein SigningKey angegeben werden.", nameof(RestCalendarClientOptions.SigningKey));
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
