namespace SDVDaily.Models
{
    public partial class HarvestCheck
    {
        public int GrowingCropId { get; set; }
        public int CropId { get; set; }
        public bool IsHarvested { get; set; }
    }
}
