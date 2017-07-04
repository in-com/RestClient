using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    public class AuthenticationFailedContext
    {
        #region Fields

        /// <summary>
        /// Gibt den URL des API-Servers.
        /// </summary>
        public readonly string ServerAddress;

        /// <summary>
        /// Gibt die Fehlermeldung.
        /// </summary>
        public readonly Exception Error;

        #endregion

        #region Properties

        /// <summary>
        /// Gibt an, ob diese Fehlermeldung abgewickelt wurde. Wenn ja, wird der <see cref="RestClient"/>
        /// keine <see cref="Exception"/> auslösen.
        /// </summary>
        public bool IsHandled { get; private set; } = false;

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="AuthenticationFailedContext"/> Klasse.
        /// </summary>
        /// <param name="endpoint">Der Endpunkt der den Fehler ausgelöst hat.</param>
        /// <param name="error">Die Fehlermeldung mit weiteren Informationen.</param>
        public AuthenticationFailedContext(string serverAddress, Exception error)
        {
            ServerAddress = serverAddress;
            Error = error;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Setzt diese Fehlermeldung auf "abgewickelt".
        /// </summary>
        public void Handled()
        {
            IsHandled = true;
        }

        #endregion
    }
}
