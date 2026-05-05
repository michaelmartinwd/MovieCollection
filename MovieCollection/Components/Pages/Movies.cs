using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class Movies : ComponentBase
    {
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;

        public MovieEntity Movie { get; set; } = new();

        public List<MovieEntity> MovieList { get; set; } = new();

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            await GetMovie("Terminator");
            MovieList = await GetMovies();
        }

        public async Task<MovieEntity> GetMovie(string title)
        {
            var movie = await MovieService.GetMovieAsync(title);
            return movie;
        }

        public async Task<List<MovieEntity>> GetMovies()
        {
            var movies = await MovieService.GetMoviesAsync();
            return movies;
        }

        public async Task SubmitMovie()
        {
            await MovieService.AddMovieAsync(Movie);
        }

    }
}