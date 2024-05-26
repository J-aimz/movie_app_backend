
using Microsoft.Extensions.Caching.Memory;
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

        //private List<string> _searchHistory = new List<string>();
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "SearchHistory";

        public OmdbService(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            //_searchHistory = new List<string>();
            _apiKey = configuration.GetSection("OmbdCredentials")["ApiKey"]!;
            _baseUrl = configuration.GetSection("OmbdCredentials")["BaseUrl"]! + $"?apikey={_apiKey}";
            _memoryCache = memoryCache;
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
            List<string> searchHistory;
            if (!_memoryCache.TryGetValue(CacheKey, out searchHistory))
            {
                searchHistory = new List<string>();
            }

            searchHistory!.Remove(title);

            if (searchHistory.Count == 5)
            {
                searchHistory.RemoveAt(0);
            }

            searchHistory.Add(title);

            _memoryCache.Set(CacheKey, searchHistory);

        }

        public List<string> GetSearchHistory()
        {
            if (!_memoryCache.TryGetValue(CacheKey, out List<string> searchHistory))
            {
                searchHistory = new List<string>();
            }
            return searchHistory;
        }
    }
}
