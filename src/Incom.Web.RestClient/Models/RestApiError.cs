using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    /// <summary>
    /// Die Hauptdatenklasse für alle Fehler die in der WebAPI auftreten.
    /// </summary>
    public class RestApiError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
