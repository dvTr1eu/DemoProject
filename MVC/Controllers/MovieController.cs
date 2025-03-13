using Application.Services;
using AutoMapper;
using Core.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class MovieController(IMovieService movieService, IMapper mapper, IShowService showService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var movies = await movieService.ListSort("default");
            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);
            return View(moviesDto);
        }

        public async Task<IActionResult> GetMoviesBySort(string sortOrder)
        {
            var movies = await movieService.ListSort(sortOrder);
            var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);
            return PartialView("_MoviesSortPartial", moviesDto);
        }

        public async Task<IActionResult> MovieDetail(int movieId)
        {
            var movie = await movieService.FindById(movieId);
            var movieDetail = mapper.Map<MovieDetailDto>(movie);
            movieDetail.GenreNames = movie.MovieTypes?.Select(mt => mt.Genre.Name).ToList() ?? new List<string>();

            var show = await showService.GetShowByMovieId(movieId);
            var showMapper = mapper.Map<IEnumerable<ShowDto>>(show);
            var distinctShowDays = showMapper.Select(s => s.ShowDay).Distinct().ToList();


            var viewModel = new MovieBookingDto
            {
                MovieDetail = movieDetail,
                Showtimes = showMapper,
                DistinctShowDays = distinctShowDays
            };
            return View(viewModel);
        }

        public async Task<IActionResult> GetShowtimesByDate(string day, int movieId)
        {
            var showtimes = await showService.GetShowByShowDayAndMovieId(day, movieId);
            var showMapper = mapper.Map<IEnumerable<ShowDto>>(showtimes);

            var result = showMapper.GroupBy(s => new { s.Room.Cinema.Id, s.Room.Cinema.Name })
                .Select(g => new
                {
                    cinemaId = g.Key.Id,
                    cinemaName = g.Key.Name,
                    showTimes = g.SelectMany(s => s.ShowTimeDetails.Select(t => new
                        {
                            showId = s.Id, 
                            time = t.ToString("HH:mm") 
                        }))
                        .OrderBy(t => t.time)
                        .ToList()
                });

            return Json(result);
        }
    }
}
