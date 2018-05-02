using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestCalendarClient
{
    /// <summary>
    /// Definiert alle möglichen Optionen des <see cref="RestCalendarClient"/>.
    /// </summary>
    public class RestCalendarClientOptions : IRestClientOptions
    {
        /// <summary>
        /// Gibt oder Setzt die REST-API Server Adresse.
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Gibt oder Setzt die Zugangsdaten.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Gibt oder setzt den PublicKey zum verifizieren der Signatur eines ID Tokens.
        /// </summary>
        public X509Certificate2 SigningKey { get; set; }

        /// <summary>
        /// Gibt oder setzt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        public AuthenticationEvents AuthenticationEvents { get; set; }

        /// <summary>
        /// Gibt oder setzt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        public EndpointEvents EndpointEvents { get; set; }

        /// <summary>
        /// Gibt oder Setzt die DataStorage Klasse für das speichern der Tokens. Standardmäßig wird
        /// der <see cref="FileDataStore"/> verwendet.
        /// </summary>
        public IDataStore DataStore { get; set; } = new FileDataStore();
    }
}
