using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace MovieCollectionAPI
{
    public class MovieLibraryAPI
    {
        private readonly IConfiguration _configuration;

        public MovieLibraryAPI(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SearchContainer<SearchMovie>> SearchMoviesAsync(string query, int page)
        {
            var apikey = _configuration.GetConnectionString("TMDB_API_KEY");
            var client = new TMDbClient(apikey!);
            var pageSize = 12;

            var searchResults = await client.SearchMovieAsync(query, page);

            if (searchResults?.Results != null)
            {
                searchResults.Results = searchResults.Results
                    .Take(pageSize)
                    .ToList();

                return searchResults;
            }

            return new SearchContainer<SearchMovie>();
        }
    }
}