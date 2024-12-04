using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class ReminderRepeat
    {
        public int Id { get; set; }
        public int? ReminderId { get; set; }
        public int? Day { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
