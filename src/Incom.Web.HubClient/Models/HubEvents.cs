using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.HubClient.Models
{
    /// <summary>
    /// Enthält mögliche Events die vom Client ausgeführt werden.
    /// </summary>
    public class HubEvents
    {
        /// <summary>
        /// Wird ausgeführt, wenn der Server seine Versionsnummer sendet.
        /// </summary>
        public Func<string, Task> OnApiVersion { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn das Nutzungsrecht an einem Grab beendet wurde.
        /// </summary>
        public Action<Exception> OnError { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn eine Verbindung mit dem Hub hergestellt wurde.
        /// </summary>
        public Func<Task> OnConnected { get; set; }

        /// <summary>
        /// Wird aufgerufen, sobald die Verbindung zum Hub getrennt wurde.
        /// </summary>
        public Func<Task> OnDisconnected { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn sich ein anderer Client am Hub verbindet.
        /// </summary>
        public Func<Client, Task> OnClientConnected { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn sich ein anderer Client am Hub abmeldet.
        /// </summary>
        public Func<Client, Task> OnClientDisconnected { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn der Server eine Nachricht sendet.
        /// </summary>
        public Func<string, Task> OnMessage { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn der Server Grabdaten sendet.
        /// </summary>
        public Func<IEnumerable<Grab>, Task> OnGraveInformation { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn der Server Grabdaten sendet.
        /// </summary>
        [Obsolete("Dieses Ereignis wird in Zukunft nicht mehr unterstützt. Verwenden Sie stattdessen \"OnGraveInformation\".")]
        public Func<IEnumerable<Grab>, Task> OnGraveDataReceived { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn ein Grab erstellt wurde.
        /// </summary>
        public Func<Grab, Task> OnGraveCreated { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn ein Grab bearbeitet wurde.
        /// </summary>
        public Func<Grab, Task> OnGraveChanged { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn ein Grab archiviert wurde.
        /// </summary>
        public Func<Grab, Grab, Task> OnGraveArchived { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn ein archviertes Grab wieder aus dem Archiv zurück geholt wird.
        /// </summary>
        public Func<Grab, Task> OnGraveSetActive { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn ein Grab gelöscht wurde.
        /// </summary>
        public Func<Grab, Task> OnGraveDeleted { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn zwei Gräber verbunden werden.
        /// </summary>
        public Func<Grab, Grab, Task> OnGraveJoined { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn zwei Gräber getrennt werden.
        /// </summary>
        public Func<Grab, Grab, Task> OnGraveDetached { get; set; }

        /// <summary>
        /// Wird ausgeführt, wenn das Nutzungsrecht an einem Grab beendet wurde.
        /// </summary>
        public Func<Grab, Task> OnGraveRightOfUseEnded { get; set; }
    }
}
