
using MovieApp.Api.Dto;
using MovieApp.Api.Services.Interfaces;
using Newtonsoft.Json;

namespace MovieApp.Api.Services.Implementations
{
    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        private List<string> _searchHistory = new List<string>();

        public OmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _searchHistory = new List<string>();
            _apiKey = configuration.GetSection("OmbdCredentials")["ApiKey"]!;
            _baseUrl = configuration.GetSection("OmbdCredentials")["BaseUrl"]! + $"?apikey={_apiKey}";
        }

        public async Task<OmdbApiResponse> SearchMoviesAsync(string title,  int page = 1)
        {
            string route = $"{_baseUrl}&s={title}&page={page.ToString()}";
            var response = await _httpClient.GetStringAsync(route);

            SaveSearchQuery(title);

            var omdbResponse = JsonConvert.DeserializeObject<OmdbApiResponse>(response);

            return omdbResponse;
        }


        public async Task<OmdbMovieDetailsResponse> GetMovieDetailsAsync(string imdbId)
        {
            string route = $"{_baseUrl}&i={imdbId.ToString()}";
            var response = await _httpClient.GetStringAsync(route);

            var omdbResponse = JsonConvert.DeserializeObject<OmdbMovieDetailsResponse>(response);

            return omdbResponse;
        }
     
        public void SaveSearchQuery(string title)
        {
            if (_searchHistory.Count == 5) 
                _searchHistory.RemoveAt(0);
            
            _searchHistory.Add(title);

        }

        public List<string> GetSearchHistory()
        {
            return _searchHistory;
        }
    }
}
