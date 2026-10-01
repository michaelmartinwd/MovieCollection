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

            var searchResults = await client.SearchMovieAsync(query, page);

            if (searchResults?.Results != null)
            {
                foreach (var result in searchResults.Results)
                    Console.WriteLine(result.Title);

                return searchResults;
            }

            return new SearchContainer<SearchMovie>();
        }
    }
}