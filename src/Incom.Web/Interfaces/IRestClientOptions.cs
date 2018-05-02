using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    public interface IRestClientOptions
    {
        /// <summary>
        /// Gibt die REST-API Server Adresse.
        /// </summary>
        string ServerAddress { get; }

        /// <summary>
        /// Gibt die Zugangsdaten.
        /// </summary>
        ICredentials Credentials { get; }

        /// <summary>
        /// Gibt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        AuthenticationEvents AuthenticationEvents { get; }

        /// <summary>
        /// Gibt Events die, wenn zugewiesen, ausgeführt werden.
        /// </summary>
        EndpointEvents EndpointEvents { get; }

        /// <summary>
        /// Gibt die DataStorage Klasse für das speichern der Tokens. Standardmäßig wird
        /// der <see cref="FileDataStore"/> verwendet.
        /// </summary>
        IDataStore DataStore { get; }
    }
}
