using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Api.Dto
{
    public class OmdbApiResponse
    {
        public List<OmdbMovieResponse> Search { get; set; }
        public string TotalResults { get; set; }
        public string Response { get; set; }
    }
}