using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incom.Web.RestClient.Models
{
    /// <summary>
    /// Definition für den Payload eines ID Tokens.
    /// </summary>
    public class JwtToken
    {
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("exp")]
        public long ExpirationTime { get; set; }

        [JsonProperty("iat")]
        public long IssuedAt { get; set; }
    }
}
