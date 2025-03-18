using Core.Entities;
using Core.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace UnitTest
{
    public class MovieServiceTest
    {
        private readonly Mock<IMovieRepository> _movieMockRepo;
        private readonly MovieService _movieService;

        public MovieServiceTest()
        {
            _movieMockRepo = new Mock<IMovieRepository>(); 
            _movieService = new MovieService(_movieMockRepo.Object);
        }
        [Fact]
        public async Task Create_ShouldReturnTrue_WhenMovieIsAddedSuccessfully()
        {
            // Arrange
            var testMovie = new Movie
            {
                Id = 1,
                Title = "The Matrix",
                Description = "A sci-fi classic",
                Director = "Lana Wachowski",
                Actors = "Keanu Reeves, Laurence Fishburne",
                Poster = "poster.jpg",
                TrailerUrl = "https://youtube.com/trailer",
                DurationMinutes = 136,
                ScreeningDay = DateTime.Now,
                Status = true,
                Language = "English",
                LimitAge = "16+",
                RatePoint = 8.7f,
                Shows = new List<Show>(),
                MovieTypes = new List<MovieType>()
            };

            _movieMockRepo.Setup(repo => repo.AddAsync(It.IsAny<Movie>()))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _movieService.Create(testMovie);

            // Assert
            Assert.True(result);
            _movieMockRepo.Verify(repo => repo.AddAsync(It.Is<Movie>(m => m.Title == "The Matrix")), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnFalse_WhenExceptionIsThrown()
        {
            // Arrange
            var testMovie = new Movie
            {
                Title = "Inception"
            };

            _movieMockRepo.Setup(repo => repo.AddAsync(It.IsAny<Movie>()))
                .ThrowsAsync(new Exception("Database error")); 

            // Act
            var result = await _movieService.Create(testMovie);

            // Assert
            Assert.False(result);
            _movieMockRepo.Verify(repo => repo.AddAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Theory]
        [InlineData(1)] 
        public async Task Delete_ShouldReturnTrue_WhenDeleteIsSuccessful(int movieId)
        {
            // Arrange
            _movieMockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _movieService.Delete(movieId);

            // Assert
            Assert.True(result);
            _movieMockRepo.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == movieId)), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        public async Task Delete_ShouldReturnFalse_WhenDeleteFails(int movieId)
        {
            // Arrange
            _movieMockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _movieService.Delete(movieId);

            // Assert
            Assert.False(result);
            _movieMockRepo.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == movieId)), Times.Once);
        }

        [Theory]
        [InlineData("default")]
        public async Task Sort_ShouldReturnListMovieSortDefault_WhenSortOrder(string sortOrder)
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "A Movie", Status = true },
                new Movie { Id = 2, Title = "B Movie", Status = true },
                new Movie { Id = 3, Title = "C Movie", Status = false },
                new Movie { Id = 4, Title = "D Movie", Status = false },
                new Movie { Id = 5, Title = "E Movie", Status = true },
                new Movie { Id = 6, Title = "F Movie", Status = true },
                new Movie { Id = 7, Title = "G Movie", Status = false },
            };

            _movieMockRepo.Setup(repo => repo.GetBySortCondition(It.IsAny<string>(), It.IsAny<Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>>>()))
                .ReturnsAsync(movies.Where(m => m.Status == true)); 

            // Act
            var result = await _movieService.ListSort(sortOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.All(result, movie => Assert.True(movie.Status));
            _movieMockRepo.Verify(repo => repo.GetBySortCondition(It.Is<string>(s => s == sortOrder), It.IsAny<Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>>>()), Times.Once);
        }

        [Theory]
        [InlineData("upcoming")]
        public async Task Sort_ShouldReturnListMovieSortUpcoming_WhenSortOrder(string sortOrder)
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "A Movie", Status = true },
                new Movie { Id = 2, Title = "B Movie", Status = true },
                new Movie { Id = 3, Title = "C Movie", Status = false },
                new Movie { Id = 4, Title = "D Movie", Status = false },
                new Movie { Id = 5, Title = "E Movie", Status = true },
                new Movie { Id = 6, Title = "F Movie", Status = true },
                new Movie { Id = 7, Title = "G Movie", Status = false },
            };

            _movieMockRepo.Setup(repo => repo.GetBySortCondition(It.IsAny<string>(), It.IsAny<Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>>>()))
                .ReturnsAsync(movies.Where(m => m.Status == false));

            // Act
            var result = await _movieService.ListSort(sortOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, movie => Assert.False(movie.Status));
            _movieMockRepo.Verify(repo => repo.GetBySortCondition(It.Is<string>(s => s == sortOrder), It.IsAny<Func<IQueryable<Movie>, IIncludableQueryable<Movie, object>>>()), Times.Once);
        }
    }
}