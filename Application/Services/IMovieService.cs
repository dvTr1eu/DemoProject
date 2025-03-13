using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Application.Services
{
    public interface IMovieService : IServiceBase<Movie, int>
    {
        Task<IEnumerable<Movie>> ListSort(string? sortOrder);
    }
}