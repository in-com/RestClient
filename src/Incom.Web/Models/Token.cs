using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.Models
{
    /// <summary>
    /// Definition eines Token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Enthält bei der WinFriedSE-API das eigentliche access_token und bei der Terminplaner-API
        /// ein ID Token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Ist immer Bearer.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gibt an wann das Token abläuft. (In Sekunden)
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gibt an wann das Token erstellt wurde. Wird nur bei der WinFriedSE-API verwendet.
        /// </summary>
        [JsonProperty("issued", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTimeOffset? Issued { get; set; }
    }
}
