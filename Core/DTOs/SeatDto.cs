using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public char SeatRow { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
        public bool Status { get; set; } = false;
    }
}
