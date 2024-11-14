namespace SDVDaily.Models
{
    public class GrowingCropViewModel
    {
        public int Id { get; set; }
        public int SaveId { get; set; }
        public int CropId { get; set; }
        public string CropName { get; set; }
        public int NextHarvest { get; set; }
        public int NextHarvestSeason { get; set; }
        public int Amount { get; set; }
        public bool IsOnGinger { get; set; }
        public bool IsIndoors { get; set; }
        public bool IsAgriculturist { get; set; }
        public bool IsSG { get; set; }
        public bool IsDSG { get; set; }
        public bool IsHSG { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
