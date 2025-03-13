using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
        public DateTime BookingTime { get; set; }
        public int TotalAmount { get; set; }
        public Payment Payment { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; }
    }
}
