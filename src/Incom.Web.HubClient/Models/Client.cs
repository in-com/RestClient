using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incom.Web.HubClient.Models
{
    public class Client
    {
        [JsonProperty("conntection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_type")]
        public string ClientType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
