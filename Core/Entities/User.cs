using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<Booking> Bookings { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}

