using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.RestClient
{
    /// <summary>
    /// Die Hauptdatenklasse für unbehandelte Fehler die auf Server Ebene auftreten.
    /// </summary>
    public class InternalServerError
    {
        [JsonProperty("log_id")]
        public Guid Id { get; set; }

        [JsonProperty("log_message")]
        public string Message { get; set; }
    }
}
