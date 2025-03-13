using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeatCapacity { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Show> Shows { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }

}
