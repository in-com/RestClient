using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    /// <summary>
    /// Standard abstrakte Klasse eines AuthenticationProviders. Definiert Funktionen die jeder
    /// AuthenticationProvider der dieser Klasse ableitet, implementiert werden müssen.
    /// </summary>
    public abstract class AuthenticationProvider
    {
        #region Properties

        /// <summary>
        /// Gibt die Optionen der <see cref="RestClient"/> Instanz.
        /// </summary>
        public RestClientOptions Options { get; private set; }

        /// <summary>
        /// Gibt einen Wert der Angibt ob die Authorisierung am API-Server erfolgreich war.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        /// Enthält das Access Token bei erfolgreicher Authorisierung.
        /// </summary>
        public Token AccessToken { get; private set; }

        /// <summary>
        /// Enthält eine Fehlermeldung bei erfolgloser Authorisierung.
        /// </summary>
        public RestApiException Error { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der abstrakten Klasse <see cref="AuthenticationProvider"/>.
        /// </summary>
        /// <param name="options"></param>
        public AuthenticationProvider(RestClientOptions options)
        {
            Options = options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Methode zur Authorisierung an einem API-Server.
        /// </summary>
        public abstract Task AuthorizeAsync();

        /// <summary>
        /// Authorisierung war erfolgreich.
        /// </summary>
        /// <param name="token">Das Access Token welches bei einer erfolgreichen Authorisierung zurückgesendet wurde.</param>
        public void Validated(Token token)
        {
            IsValidated = true;
            Error = null;
            AccessToken = token;
        }

        /// <summary>
        /// Authorisierung war erfolglos.
        /// </summary>
        /// <param name="code">HTTP Status Code</param>
        /// <param name="errorMessage">Fehlermeldung</param>
        public void Rejected(HttpStatusCode code, string errorMessage)
        {
            IsValidated = false;
            Error = new RestApiException(errorMessage, code);
            AccessToken = null;
        }

        #endregion
    }
}
