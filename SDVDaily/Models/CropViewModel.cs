namespace SDVDaily.Models
{
	public class CropViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		
		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }

		public int? GrowthTime { get; set; }
		public int? RegrowthTime { get; set; }
		public int? Unirrigated { get; set; }
		public bool IsWalkable { get; set; }
		public int? StartYear { get; set; }
		public int? SellPrice { get; set; }
		public string? Img { get; set; }

        public string? Seasons { get; set; }

        public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool? IsDeleted { get; set; }
	}
}
