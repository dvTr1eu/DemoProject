using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SeatLock
    {
        public int Id { get; set; }
        public string SeatCode { get; set; } // VD: A1, B2
        public int RoomId { get; set; }
        public int ShowId { get; set; }
        public string UserId { get; set; }
        public DateTime LockTime { get; set; }
        public bool IsLocked { get; set; }
    }
}
