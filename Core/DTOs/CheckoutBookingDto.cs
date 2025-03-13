using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CheckoutBookingDto
    {
        public string FullName { get; set; }
        public int ShowId { get; set; }
        public DateTime BookingTime { get; set; }
        public int TotalAmount { get; set; }
    }
}
