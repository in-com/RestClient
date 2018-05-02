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
    public abstract class HubProvider<TOptions>
    {
        #region Properties

        /// <summary>
        /// Gibt die Optionen des Hub Clients.
        /// </summary>
        public TOptions Options { get; private set; }

        /// <summary>
        /// Gibt einen Wert der angibt, ob eine Verbindung zum Hub besteht.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Enthält eine Fehlermeldung bei erfolgloser Authorisierung.
        /// </summary>
        public RestApiException Error { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der abstrakten Klasse <see cref="HubProvider{TOptions}"/>.
        /// </summary>
        /// <param name="options"></param>
        public HubProvider(TOptions options)
        {
            Options = options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Methode für das herstellen einer Verbindung zum Hub.
        /// </summary>
        /// <param name="cookies">Cookies die zur Authentifizierung am Hub verwendet werden.</param>
        public abstract Task ConnectAsync(IEnumerable<Cookie> cookies);

        /// <summary>
        /// Schließt eine vorhandene Verbindung.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Verbindung erfolgreich hergestellt.
        /// </summary>
        public void Connected()
        {
            IsConnected = true;
            Error = null;
        }

        /// <summary>
        /// Verbindung getrennt.
        /// </summary>
        public void Disconnected()
        {
            IsConnected = false;
        }

        /// <summary>
        /// Authorisierung war erfolglos.
        /// </summary>
        /// <param name="code">HTTP Status Code</param>
        /// <param name="errorMessage">Fehlermeldung</param>
        public void Rejected(HttpStatusCode code, string errorMessage)
        {
            IsConnected = false;
            Error = new RestApiException(errorMessage, code);
        }

        #endregion
    }
}
