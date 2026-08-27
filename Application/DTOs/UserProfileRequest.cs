using System.Collections.Generic;

namespace Application.DTOs
{
    public class UserProfileRequest
    {
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public Dictionary<string, string> Preferencias { get; set; } = new();
    }
}
