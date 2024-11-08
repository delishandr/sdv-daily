using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class GrowingCrop
    {
        public int Id { get; set; }
        public int SaveId { get; set; }
        public int CropId { get; set; }
        public int NextHarvest { get; set; }
        public int Amount { get; set; }
        public bool IsOnGinger { get; set; }
        public bool IsIndoors { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
