using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.Models
{
    /// <summary>
    /// Enthält mögliche Events die vom Client ausgeführt werden.
    /// </summary>
    public class EndpointEvents
    {
        /// <summary>
        /// Wird vom Client aufgerufen sobald eine Anfrage an den API-Server fehlgeschlagen ist.
        /// </summary>
        public Func<EndpointFailedContext, Task> OnRequestFailed { get; set; }
    }
}
