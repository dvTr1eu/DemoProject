using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Poster { get; set; }
        public string TrailerUrl { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ScreeningDay { get; set; }
        public bool Status { get; set; }
        public string Language { get; set; }
        public string LimitAge { get; set; }
        public float RatePoint { get; set; }
        public virtual ICollection<Show> Shows { get; set; }
        public ICollection<MovieType> MovieTypes { get; set; }
    }

}
