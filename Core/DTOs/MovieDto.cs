using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string TrailerUrl { get; set; }
        public string? Poster { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ScreeningDay { get; set; }
        public bool Status { get; set; }
        public string Language { get; set; }
        public string LimitAge { get; set; }
        public float RatePoint { get; set; }
        public ICollection<Show>? Shows { get; set; } = null;
        public ICollection<MovieType>? MovieTypes { get; set; } = null;
        public List<int>? GenreId { get; set; }
        public List<string>? GenreNames { get; set; }
    }
}