using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Api.Dto;

namespace MovieApp.Api.Services.Interfaces
{
    public interface IOmdbService
    {
        Task<OmdbApiResponse> SearchMoviesAsync(string title, int page);
        Task<OmdbMovieDetailsResponse> GetMovieDetailsAsync(string imdbId);
        List<string> GetSearchHistory();
    }
}
