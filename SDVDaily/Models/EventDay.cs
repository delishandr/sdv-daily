using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class EventDay
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? Day { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
