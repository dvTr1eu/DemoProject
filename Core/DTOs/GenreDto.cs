using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class GenreDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<MovieType> MovieTypes { get; set; }

    }
}
