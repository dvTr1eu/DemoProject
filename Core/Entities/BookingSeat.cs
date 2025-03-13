using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BookingSeat
    {
        public int BookId { get; set; }
        public Booking Booking { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int ShowTimeId { get; set; }
        public ShowtimeDetail ShowtimeDetail { get; set; }
    }
}