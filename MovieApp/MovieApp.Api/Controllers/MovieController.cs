using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Api.Controllers
{
    [ApiController]
    [Route("api-v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private ILogger<MovieController> _logger;


        [HttpGet("get-movie")]
        public async Task<IActionResult> GetMovieAsync(string movieName)
        {

            return Ok(movieName);
        }
    }
}