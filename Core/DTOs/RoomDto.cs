using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeatCapacity { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public ICollection<Show>? Shows { get; set; } = null;
        public ICollection<Seat>? Seats { get; set; } = null;
    }
}
