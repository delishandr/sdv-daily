using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class CropSeason
    {
        public int Id { get; set; }
        public int? CropId { get; set; }
        public int? SeasonId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
