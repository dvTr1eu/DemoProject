using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
        public DateTime BookingTime { get; set; }
        public int TotalAmount { get; set; }
        public Payment Payment { get; set; }
        public ICollection<BookingSeat>? BookingSeats { get; set; } = null;
    }
}