using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    /// <summary>
    /// Standard abstrakte Klasse eines AuthenticationProviders. Definiert Funktionen die jeder
    /// AuthenticationProvider der dieser Klasse ableitet, implementiert werden müssen.
    /// </summary>
    public abstract class AuthenticationProvider<TOptions>
    {
        #region Properties

        /// <summary>
        /// Gibt die Optionen des Rest Clients.
        /// </summary>
        public TOptions Options { get; private set; }

        /// <summary>
        /// Gibt einen Wert der angibt, ob die Authorisierung am API-Server erfolgreich war.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        /// Gibt ein <see cref="AuthenticationContext"/> Objekt mit Daten zur erfolgreichen Authorisierung.
        /// </summary>
        public AuthenticationContext Context { get; private set; }

        /// <summary>
        /// Enthält eine Fehlermeldung bei erfolgloser Authorisierung.
        /// </summary>
        public RestApiException Error { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der abstrakten Klasse <see cref="AuthenticationProvider{TOptions}"/>.
        /// </summary>
        /// <param name="options"></param>
        public AuthenticationProvider(TOptions options)
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
        /// <param name="cookies">Cookies welche bei einer erfolgreichen Authorisierung zurückgesendet wurden.</param>
        public void Validated(Token token, IEnumerable<Cookie> cookies = null)
        {
            IsValidated = true;
            Error = null;
            Context = new AuthenticationContext(accessToken: token, cookies: cookies);
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
            Context = null;
        }

        #endregion
    }
}
