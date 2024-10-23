namespace SDVDaily.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Location { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Preparation { get; set; }

        public List<string?> Days { get; set; } = new List<string?>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
