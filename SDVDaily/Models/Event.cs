using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Preparation { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
