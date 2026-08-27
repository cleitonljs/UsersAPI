using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public Dictionary<string, string> Preferencias { get; set; } = new();
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
    }
}
