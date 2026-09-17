using Microsoft.AspNetCore.Http.HttpResults;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace MovieCollectionAPI
{
    public class MovieLibraryAPI
    {
        public async Task<List<SearchMovie>> SearchMoviesAsync(string query, int page = 1)
        {
            var client = new TMDbClient("YOUR_API_KEY");

            var searchResults = await client.SearchMovieAsync(query, page);

            foreach (var result in searchResults.Results)
                Console.WriteLine(result.Title);

            return searchResults.Results;
        }
    }
}