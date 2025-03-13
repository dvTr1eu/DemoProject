using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.DTOs
{
    public class BookingViewDto
    {
        public int ShowId { get; set; }
        public string MovieTitle { get; set; }
        public TimeOnly ShowTime { get; set; }
        public string ShowDay { get; set; }
        public string RoomName { get; set; }
        public string CinemaName { get; set; }
        public int TicketPrice { get; set; }
        public int? RoomId { get; set; }
        public int SeatQuantity { get; set; }
        public List<Seat> Seats { get; set; }
        public List<Seat> BookedSeats { get; set; }
    }
}
