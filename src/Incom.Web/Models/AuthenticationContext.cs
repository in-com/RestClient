using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.Models
{
    public class AuthenticationContext
    {
        #region Properties
        
        public Token AccessToken { get; private set; }

        public IEnumerable<Cookie> Cookies { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="AuthenticationContext"/> Klasse.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="cookies"></param>
        public AuthenticationContext(Token accessToken = null, IEnumerable<Cookie> cookies = null)
        {
            AccessToken = accessToken;
            Cookies = cookies;
        }

        #endregion
    }
}
