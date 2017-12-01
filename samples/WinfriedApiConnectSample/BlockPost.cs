using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class BlockPost
    {
        /// <summary>
        /// Gibt oder setzt die ID eines Kalenders.
        /// </summary>
        [JsonProperty("calendar_id")]
        public string CalendarId { get; set; }

        /// <summary>
        /// Gibt oder setzt das Beginndatum.
        /// </summary>
        [JsonProperty("starttime")]
        public DateTimeOffset? Starttime { get; set; }

        /// <summary>
        /// Gibt oder setzt das Endedatum.
        /// </summary>
        [JsonProperty("endtime")]
        public DateTimeOffset? Endtime { get; set; }

        /// <summary>
        /// Gibt oder setzt die Begründung der Sperre.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
