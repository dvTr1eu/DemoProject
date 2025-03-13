using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class AuthResponse
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public IdentityUser SignInUser { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
