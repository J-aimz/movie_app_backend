using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Api.Dto;
using MovieApp.Api.Services.Interfaces;

namespace MovieApp.Api.Controllers
{
    [ApiController]
    [Route("api-v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IOmdbService _omdbService;

        public MovieController(ILogger<MovieController> logger, IOmdbService omdbService)
        {
            _logger = logger;
            _omdbService = omdbService;
        }

        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OmdbApiResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SerachForMovieByTitle([FromQuery] string title, [FromQuery] int page = 1)
        {
            if (string.IsNullOrEmpty(title))
                return BadRequest(ApiResponse.Failed("Bad Request"));

            try
            {
                _logger.LogInformation("Initiated movie search with title: {title} at time: {time} ", title, DateTime.UtcNow);
                var result = await _omdbService.SearchMoviesAsync(title, page);

                if (result is null)
                {
                    _logger.LogInformation("Search word: {title} not found at time: {time}", title, DateTime.UtcNow);
                    return NotFound(ApiResponse.Failed("Not Found"));
                }

                ApiResponse<OmdbApiResponse>.Success(result);
                _logger.LogInformation("Returning search result at time: {time}", DateTime.UtcNow);
                return Ok(ApiResponse<OmdbApiResponse>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("Server with error: {err} at time: {time}", ex, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Failed("Internal server error"));
            }
        }

        [HttpGet("{imdbId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OmdbMovieDetailsResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovieById(string imdbId)
        {
            if (string.IsNullOrEmpty(imdbId))
                return BadRequest(ApiResponse.Failed("Bad Request"));

            try
            {
                var result = await _omdbService.GetMovieDetailsAsync(imdbId);
                if (result is null)
                {
                    _logger.LogInformation("Movie not found with id: {imdbId} at time: {time}", imdbId, DateTime.UtcNow);
                    return NotFound(ApiResponse.Failed("Not Found"));
                }

                ApiResponse<OmdbMovieDetailsResponse>.Success(result);
                _logger.LogInformation("Returning movie result at time: {time}", DateTime.UtcNow);
                return Ok(ApiResponse<OmdbMovieDetailsResponse>.Success(result));

            }
            catch (Exception ex)
            {
                _logger.LogError("Server error with error: {err} at time: {time}", ex, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Failed("Internal server error"));
            }

        }

        [HttpGet("Get-Search-History")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<string>>))]
        public async Task<IActionResult> GetSearchHistory()
        {
            var result = _omdbService.GetSearchHistory();
            _logger.LogInformation("Returning search history at time: {time}", DateTime.UtcNow);
            return Ok(ApiResponse<List<string>>.Success(result));
        }
    }
}
