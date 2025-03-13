using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageMap { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Show> Shows { get; set; }

    }

}
