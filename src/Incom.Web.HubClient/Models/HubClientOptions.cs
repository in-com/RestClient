using Incom.Web.Models;
using Microsoft.AspNet.SignalR.Client;
using System.IO;

namespace Incom.Web.HubClient.Models
{
    /// <summary>
    /// Definiert alle möglichen Optionen des <see cref="HubClient"/>.
    /// </summary>
    public class HubClientOptions
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
        /// Gibt oder setzt den Namen der Verbindungen. Hier sollte immer der Name der Anwendung eingetragen werden, die sich mit
        /// dem Hub Verbindet.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gibt oder setzt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        public AuthenticationEvents AuthenticationEvents { get; set; }

        /// <summary>
        /// Gibt oder setzt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        public HubEvents HubEvents { get; set; }

        /// <summary>
        /// Gibt oder setzt eine Wert der angibt, was geloggt werden soll.
        /// </summary>
        public TraceLevels TraceLevel { get; set; } = TraceLevels.None;

        /// <summary>
        /// Gibt oder setzt den Trace/Log Output.
        /// </summary>
        public TextWriter TraceWriter { get; set; }
    }
}
