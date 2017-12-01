using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Appointment
    {
        #region Properties
        
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("sync_id")]
        public int? SyncId { get; set; }
        
        [JsonProperty("sync_person_id")]
        public int? SyncPersonId { get; set; }
        
        [JsonProperty("parent_event_id")]
        public int? ParentEventId { get; set; }
        
        [JsonProperty("starttime_no_offset")]
        public DateTime Starttime { get; set; }

        [JsonProperty("starttime")]
        public DateTimeOffset StarttimeOffset { get; set; }
        
        [JsonProperty("endtime_no_offset")]
        public DateTime Endtime { get; set; }

        [JsonProperty("endtime")]
        public DateTimeOffset EndtimeOffset { get; set; }
        
        [JsonProperty("setuptime_before")]
        public int? SetuptimeBefore { get; set; }
        
        [JsonProperty("setuptime_after")]
        public int? SetuptimeAfter { get; set; }
        
        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }
        
        [JsonProperty("remark")]
        public string Remark { get; set; }
        
        [JsonProperty("remark2")]
        public string Remark2 { get; set; }
        
        [JsonProperty("funeral_guests_count")]
        public int? FuneralGuestsCount { get; set; }
        
        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }

        #endregion

        #region Properties (Calendar)
        
        [JsonProperty("calendar_id")]
        public int CalendarId { get; set; }
        
        [JsonProperty("calendar_name")]
        public string CalendarName { get; set; }
        
        [JsonProperty("calendar_normalized_name")]
        public string CalendarNormalizedName { get; set; }
        
        [JsonProperty("calendar_color")]
        public string CalendarColor { get; set; }
        
        [JsonProperty("calendar_group_id")]
        public int CalendarGroupId { get; set; }
        
        [JsonProperty("calendar_group_name")]
        public string CalendarGroupName { get; set; }
        
        [JsonProperty("calendar_normalized_group_name")]
        public string CalendarNormalizedGroupName { get; set; }

        #endregion

        #region Properties (Cementery)
        
        [JsonProperty("cementery_id")]
        public int? CementeryId { get; set; }
        
        [JsonProperty("cementery_sync_id")]
        public int? CementerySyncId { get; set; }
        
        [JsonProperty("cementery_name_short")]
        public string CementeryNameShort { get; set; }
        
        [JsonProperty("cementery_name_long")]
        public string CementeryNameLong { get; set; }

        #endregion

        #region Properties (Status)
        
        [JsonProperty("status_id")]
        public int? StatusId { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("status_icon")]
        public string StatusIcon { get; set; }
        
        [JsonProperty("status_color")]
        public string StatusColor { get; set; }
        
        [JsonProperty("status_uri_encoded")]
        public string StatusUriEncoded { get; set; }

        #endregion

        #region Properties (Subject)
        
        [JsonProperty("subject_id")]
        public int SubjectId { get; set; }
        
        [JsonProperty("subject")]
        public string Subject { get; set; }
        
        [JsonProperty("subject_type_id")]
        public int SubjectTypeId { get; set; }

        #endregion

        #region Properties (Burial Method)
        
        [JsonProperty("burial_method_id")]
        public int? BurialMethodId { get; set; }
        
        [JsonProperty("burial_method")]
        public string BurialMethod { get; set; }
        
        [JsonProperty("burial_method_short")]
        public string BurialMethodShort { get; set; }

        #endregion

        #region Properties (Grave)
        
        [JsonProperty("grave_no")]
        public string GraveNo { get; set; }
        
        [JsonProperty("grave_type")]
        public string GraveType { get; set; }

        #endregion

        #region Properties (ViewComponent Board)
        
        [JsonProperty("is_available_time")]
        public bool IsAvailableTime { get; set; }
        
        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }
        
        [JsonProperty("blocked_description")]
        public string BlockedDescription { get; set; }

        #endregion

        #region Properties (User)
        
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        
        [JsonProperty("user_firstname")]
        public string UserFirstname { get; set; }
        
        [JsonProperty("user_lastname")]
        public string UserLastname { get; set; }
        
        [JsonProperty("user_fullname")]
        public string UserFullname { get; set; }
        
        [JsonProperty("user_email")]
        public string UserEmail { get; set; }
        
        [JsonProperty("user_avatar")]
        public string UserAvatar { get; set; }

        #endregion

        #region Properties (Display Type)
        
        [JsonProperty("display_type_id")]
        public int? DisplayTypeId { get; set; }
        
        [JsonProperty("display_type_text")]
        public string DisplayTypeText { get; set; }
        
        [JsonProperty("display_type_pattern")]
        public string DisplayTypePattern { get; set; }

        #endregion
    }
}
