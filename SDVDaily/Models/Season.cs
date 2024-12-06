﻿using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class Season
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
