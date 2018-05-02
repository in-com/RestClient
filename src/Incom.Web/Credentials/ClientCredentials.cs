using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web
{
    public class ClientCredentials : ICredentials
    {
        #region Properties

        /// <summary>
        /// Prüft ob die ID und das Secret gefüllt sind.
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret))
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Gibt oder setzt eine Client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gibt oder setzt ein Client Secret.
        /// </summary>
        public string ClientSecret { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gibt die Zugangsdaten.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, string> GetCredentials()
        {
            return new KeyValuePair<string, string>(ClientId, ClientSecret);
        }

        #endregion
    }
}
