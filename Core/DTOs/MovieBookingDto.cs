using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class MovieBookingDto
    {
        public MovieDetailDto MovieDetail { get; set; }
        public IEnumerable<ShowDto> Showtimes { get; set; }
        public List<DateTime> DistinctShowDays { get; set; }
    }
}
