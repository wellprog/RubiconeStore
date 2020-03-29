using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Model
{
    public class UserSession
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string SessionToken { get; set; } = Guid.NewGuid().ToString();
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;

        /************************************************/
        // Relations
        [JsonIgnore]
        public User User { get; set; }
    }
}
