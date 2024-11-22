using System;
using System.Collections.Generic;

namespace SDVDaily.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public int? LastSave { get; set; }
        public int? LoginAttempt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
