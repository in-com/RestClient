using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    /// <summary>
    /// Schnittstelle die ein Model definiert, welches für das ermitteln von Zugangsdaten zuständig ist.
    /// </summary>
    public interface ICredentials
    {
        /// <summary>
        /// Prüft ob Zugangsdaten vorhanden sind.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gibt die Zugangsdaten.
        /// </summary>
        /// <returns></returns>
        KeyValuePair<string, string> GetCredentials();
    }
}
