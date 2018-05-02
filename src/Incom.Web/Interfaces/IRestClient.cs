using Incom.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    public interface IRestClient
    {
        /// <summary>
        /// Gibt die Optionen des Clients.
        /// </summary>
        IRestClientOptions Options { get; }

        /// <summary>
        /// Gibt einen Wert der Angibt ob die Authorisierung am API-Server erfolgreich war.
        /// </summary>
        bool IsValidated { get; }

        /// <summary>
        /// Gibt den <see cref="AuthenticationContext"/> der Verbindung.
        /// </summary>
        AuthenticationContext Context { get; }

        /// <summary>
        /// Stellt eine Verbindung zum API-Server her.
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();

        /// <summary>
        /// Ändert die API Serveradresse in den <see cref="IRestClientOptions"/>.
        /// </summary>
        /// <param name="serverAddress">Die neue Adresse.</param>
        void SetServerAddress(string serverAddress);

        /// <summary>
        /// Ändert die Anmeldeinformationen in den <see cref="IRestClientOptions"/>.
        /// </summary>
        /// <param name="cliendId">Die neue ClientId.</param>
        /// <param name="clientSecret">Das neue Client Secret.</param>
        void SetClientCredentials(string cliendId, string clientSecret);
    }
}
