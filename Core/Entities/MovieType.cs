using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MovieType
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}