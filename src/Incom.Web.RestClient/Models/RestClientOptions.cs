﻿using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    /// <summary>
    /// Definiert alle möglichen Optionen des <see cref="RestClient"/>.
    /// </summary>
    public class RestClientOptions : IRestClientOptions
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
