using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class Reminder
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? NextRemind { get; set; }
        public int? NextRemindSeason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
