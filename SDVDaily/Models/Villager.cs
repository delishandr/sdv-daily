using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class Villager
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? BirthDay { get; set; }
        public int? BirthMonth { get; set; }
        public string? LovedGifts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
