using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.Models
{
    /// <summary>
    /// Enthält mögliche Events die von einem RestClient ausgeführt werden.
    /// </summary>
    public class AuthenticationEvents
    {
        /// <summary>
        /// Wird vom <see cref="RestClient"/> aufgerufen sobald das ermitteln eines Tokens/ID Tokens erfolgreich war.
        /// </summary>
        public Func<AuthenticationContext, Task> OnAuthorized { get; set; }

        /// <summary>
        /// Wird vom <see cref="RestClient"/> aufgerufen wenn das ermitteln eines Tokens/ID Tokens fehlgeschlagen ist.
        /// </summary>
        public Func<AuthenticationFailedContext, Task> OnAuthorizationFailed { get; set; }
    }
}
