﻿
namespace Core.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MovieType> MovieTypes { get; set; }
    }

}
