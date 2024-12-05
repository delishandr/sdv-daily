using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class ReminderViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public int? NextRemind { get; set; }
        public int? NextRemindYear { get; set; }
        public int? NextRemindSeason { get; set; }
        public int NextRemindDay { get; set; }

        public string? RemindType { get; set; }
        public string? FreqType { get; set; }
        public int[] Frequency { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? SaveId { get; set; }
    }
}
