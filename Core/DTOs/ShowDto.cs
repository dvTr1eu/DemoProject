using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ShowDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public DateTime ShowDay { get; set; }
        public List<TimeOnly> ShowTimeDetails { get; set; }
        public int TicketPrice { get; set; }
        public List<DateTime>? DistinctShowDays { get; set; } = null;
        public ICollection<Booking>? Bookings { get; set; } = null;
    }
}
