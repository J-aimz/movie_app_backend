////namespace MovieApp.Tests
////{
////    public class UnitTest1
////    {
////        [Fact]
////        public void Test1()
////        {

////        }
////    }
////}



//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using Xunit;
//using MovieApp.Api.Services.Implementations;
//using MovieApp.Api.Services.Interfaces;

//namespace MovieApp.Tests
//{
//    public class OmdbServiceTests
//    {
//        private readonly Mock<HttpClient> _httpClientMock;
//        private readonly Mock<IConfiguration> _configurationMock;
//        private readonly IMemoryCache _memoryCache;
//        private readonly IOmdbService _omdbService;

//        public OmdbServiceTests()
//        {
//            _httpClientMock = new Mock<HttpClient>();
//            _configurationMock = new Mock<IConfiguration>();

//            // Setup the memory cache
//            _memoryCache = new MemoryCache(new MemoryCacheOptions());

//            // Setup the configuration mock
//            var inMemorySettings = new Dictionary<string, string>
//            {
//                {"OmbdCredentials:ApiKey", "fake-api-key"},
//                {"OmbdCredentials:BaseUrl", "http://fakeurl.com"}
//            };

//            IConfiguration configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(inMemorySettings)
//                .Build();

//            _omdbService = new OmdbService(_httpClientMock.Object, configuration, _memoryCache);
//        }

//        [Fact]
//        public void OmdbService_Constructor_SetsBaseUrlAndApiKey()
//        {
//            // Arrange & Act done in constructor

//            // Assert
//            var omdbService = (OmdbService)_omdbService;
//            Assert.Equal("fake-api-key", omdbService.GetType().GetField("_apiKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(omdbService));
//            Assert.Equal("http://fakeurl.com?apikey=fake-api-key", omdbService.GetType().GetField("_baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(omdbService));
//        }

//        [Fact]
//        public void SaveSearchQuery_SavesQueryToCache()
//        {
//            // Arrange
//            var query = "test-query";

//            // Act
//            _omdbService.SaveSearchQuery(query);

//            // Assert
//            List<string> cachedSearchHistory = _memoryCache.Get<List<string>>(CacheKey);
//            Assert.NotNull(cachedSearchHistory);
//            Assert.Single(cachedSearchHistory);
//            Assert.Equal(query, cachedSearchHistory[0]);
//        }

//        [Fact]
//        public void GetSearchHistory_ReturnsSearchHistoryFromCache()
//        {
//            // Arrange
//            var searchHistory = new List<string> { "query1", "query2" };
//            _memoryCache.Set(CacheKey, searchHistory);

//            // Act
//            var result = _omdbService.GetSearchHistory();

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(searchHistory.Count, result.Count);
//            Assert.Equal(searchHistory[0], result[0]);
//            Assert.Equal(searchHistory[1], result[1]);
//        }

//        [Fact]
//        public void SaveSearchQuery_MaintainsHistorySize()
//        {
//            // Arrange
//            var initialQueries = new List<string> { "query1", "query2", "query3", "query4", "query5" };
//            _memoryCache.Set(CacheKey, initialQueries);

//            // Act
//            _omdbService.SaveSearchQuery("query6");

//            // Assert
//            var result = _omdbService.GetSearchHistory();
//            Assert.Equal(5, result.Count);
//            Assert.DoesNotContain("query1", result);
//            Assert.Equal("query2", result[0]);
//        }

//        private const string CacheKey = "SearchHistory";
//    }
//}
