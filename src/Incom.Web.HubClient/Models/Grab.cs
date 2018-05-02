using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incom.Web.HubClient.Models
{
    public class Grab
    {
        [JsonProperty("grab_id")]
        public int GrabId { get; set; }

        [JsonProperty("grab_nummer")]
        public string GrabNummer { get; set; }

        [JsonProperty("grab_name")]
        public string GrabName { get; set; }

        [JsonProperty("friedhof_id")]
        public int FriedhofId { get; set; }

        [JsonProperty("friedhof_nummer")]
        public string FriedhofNummer { get; set; }

        [JsonProperty("friedhof_name")]
        public string FriedhofName { get; set; }

        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
