using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class SaveFileViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public bool HasPet { get; set; }
        public bool HasFarmAnimals { get; set; }
        public bool IsAgriculturist { get; set; }
        public int Day { get; set; }
        public int Season { get; set; }
        public string SeasonName { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
