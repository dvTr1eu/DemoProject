using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public DateTime ShowDay { get; set; }
        public List<ShowtimeDetail> ShowTimeDetails { get; set; }
        public int TicketPrice { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
