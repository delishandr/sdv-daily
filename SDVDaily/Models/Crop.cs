using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class Crop
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int? GrowthTime { get; set; }
        public int? RegrowthTime { get; set; }
        public int? Unirrigated { get; set; }
        public bool? IsWalkable { get; set; }
        public int? StartYear { get; set; }
        public int? SellPrice { get; set; }
        public string? Img { get; set; }

		public List<CropSeason>? Seasons { get; set; }

		public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
