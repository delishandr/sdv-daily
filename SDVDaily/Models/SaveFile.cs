using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class SaveFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public bool HasPet { get; set; }
        public bool HasFarmAnimals { get; set; }
        public int Day { get; set; }
        public int Season { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
