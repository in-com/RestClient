using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Incom.Web.Models
{
    /// <summary>
    /// Die Hauptdatenklasse für alle Fehler die in der WebAPI auftreten.
    /// </summary>
    public class RestApiError
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("errors")]
        public List<RestApiErrorDetails> Errors { get; set; }
    }

    public class RestApiErrorDetails
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("location_type")]
        public string LocationType { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
