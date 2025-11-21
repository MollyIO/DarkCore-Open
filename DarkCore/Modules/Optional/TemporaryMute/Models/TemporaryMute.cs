using System;

namespace DarkCore.Modules.Optional.TemporaryMute.Models
{
    public class TemporaryMute
    {
        public string UserId { get; set; }
        public DateTime MuteEndTime { get; set; }
        public string Reason { get; set; }
    }
}