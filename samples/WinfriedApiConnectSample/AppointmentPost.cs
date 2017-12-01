using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class AppointmentPost
    {
        #region Properties (Event)

        [JsonProperty("parent_event_id")]
        public int? ParentEventId { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID's von Labels.
        /// </summary>
        [JsonProperty("label_ids")]
        public List<int> LabelIds { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID eines Labels vom Typ "Anzeigen als".
        /// </summary>
        [JsonProperty("display_type_id")]
        public int? DisplayTypeId { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID eines Kalenders.
        /// </summary>
        [JsonProperty("calendar_id")]
        public int? CalendarId { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID eines Betreffs.
        /// </summary>
        [JsonProperty("subject_id")]
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID eines Statuseintrags.
        /// </summary>
        [JsonProperty("status_Id")]
        public int? StatusId { get; set; }

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
        /// Gibt oder setzt die Rüstzeit davor in Minuten.
        /// </summary>
        [JsonProperty("setuptime_before")]
        public int? SetuptimeBefore { get; set; }

        /// <summary>
        /// Gibt oder setzt die Rüstzeit danach in Minuten.
        /// </summary>
        [JsonProperty("setuptime_after")]
        public int? SetuptimeAfter { get; set; }

        /// <summary>
        /// Gibt oder setzt eine Bemerkung.
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("remark2")]
        public string Remark2 { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID einer Bestattungsart.
        /// </summary>
        [JsonProperty("burial_method")]
        public int? BurialMethod { get; set; }

        /// <summary>
        /// Gibt oder setzt eine Grabnummer.
        /// </summary>
        [JsonProperty("grave_no")]
        public string GraveNo { get; set; }

        /// <summary>
        /// Gibt oder setzt eine Grabart.
        /// </summary>
        [JsonProperty("grave_type")]
        public string GraveType { get; set; }

        /// <summary>
        /// Gibt oder setzt die ID eines Bestatters.
        /// </summary>
        [JsonProperty("mortician_id")]
        public int? MorticianId { get; set; }

        [JsonProperty("funeral_guests_count")]
        public int? FuneralGuestsCount { get; set; }

        #endregion

        #region Properties (Deceased Person)

        /// <summary>
        /// Gibt oder setzt die Anrede einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_title")]
        public string DeceasedTitle { get; set; }

        /// <summary>
        /// Gibt oder setzt den Vornamen einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_firstname")]
        public string DeceasedFirstname { get; set; }

        /// <summary>
        /// Gibt oder setzt den Geburtsnamen einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_birthname")]
        public string DeceasedBirthname { get; set; }

        /// <summary>
        /// Gibt oder setzt den Nachnamen einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_lastname")]
        public string DeceasedLastname { get; set; }

        /// <summary>
        /// Gibt oder setzt die Straße einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_street")]
        public string DeceasedStreet { get; set; }

        /// <summary>
        /// Gibt oder setzt die Hausnummer einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_street_no")]
        public string DeceasedStreetNo { get; set; }

        /// <summary>
        /// Gibt oder setzt das Land einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_country")]
        public string DeceasedCountry { get; set; }

        /// <summary>
        /// Gibt oder setzt die Postleitzahl einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_zip_code")]
        public string DeceasedZipCode { get; set; }

        /// <summary>
        /// Gibt oder setzt den Wohnort einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_city")]
        public string DeceasedCity { get; set; }

        /// <summary>
        /// Gibt oder setzt das Geburtsdatum einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_date_of_birth")]
        public DateTime? DeceasedDateOfBirth { get; set; }

        /// <summary>
        /// Gibt oder setzt das Sterbedatum einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_date_of_death")]
        public DateTime? DeceasedDateOfDeath { get; set; }

        /// <summary>
        /// Gibt oder setzt den Sterbeort einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_place_of_death")]
        public string DeceasedPlaceOfDeath { get; set; }

        /// <summary>
        /// Gibt oder setzt die Konfession einer verstorbenen Person.
        /// </summary>
        [JsonProperty("deceased_denomination_id")]
        public int? DeceasedDenominationId { get; set; }

        #endregion

        #region Properties (Customer)

        /// <summary>
        /// Gibt oder setzt die Anrede eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_title")]
        public string CustomerTitle { get; set; }

        /// <summary>
        /// Gibt oder setzt den Vornamen eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_firstname")]
        public string CustomerFirstname { get; set; }

        /// <summary>
        /// Gibt oder setzt den Nachnamen eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_lastname")]
        public string CustomerLastname { get; set; }

        /// <summary>
        /// Gibt oder setzt die Straße eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_street")]
        public string CustomerStreet { get; set; }

        /// <summary>
        /// Gibt oder setzt die Hausnummer eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_street_no")]
        public string CustomerStreetNo { get; set; }

        /// <summary>
        /// Gibt oder setzt das Land eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_country")]
        public string CustomerCountry { get; set; }

        /// <summary>
        /// Gibt oder setzt die Postleitzahl eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_zip_code")]
        public string CustomerZipCode { get; set; }

        /// <summary>
        /// Gibt oder setzt den Wohnort eines Auftraggebers.
        /// </summary>
        [JsonProperty("customer_city")]
        public string CustomerCity { get; set; }

        #endregion

        #region Properties (Comment)

        /// <summary>
        /// Gibt oder setzt einen Kommentar.
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }

        #endregion
    }
}
