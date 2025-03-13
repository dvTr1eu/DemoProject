using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ShowtimeDetail
    {
        public int Id { get; set; }
        public TimeOnly ShowTime { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
    }
}
