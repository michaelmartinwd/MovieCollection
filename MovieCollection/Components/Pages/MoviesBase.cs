using Microsoft.AspNetCore.Components;
using MovieCollectionAPI;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class MoviesBase : ComponentBase
    {
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;
        //[Inject]
        //private IUserService UserService { get; set; } = null!;
        [Inject]
        private MovieLibraryAPI MovieLibraryAPI { get; set; } = null!;

        public MovieEntity Movie { get; set; } = new();

        public List<MovieEntity> MovieList { get; set; } = new();
        public List<MovieSearchEntity> MovieSearchList { get; set; } = new();
        public MovieSearchEntity MovieSearch { get; set; } = new();

        //public ICollection<MovieEntity>? FavoriteMovies { get; set; }

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            //var user = await UserService.GetUserAsync("TestUser");
            //FavoriteMovies = user!.FavoriteMovies;
            await SearchMovies();
            StateHasChanged();
        }

        // change the UI for the button to show that the movie has been added to favorites (rename button to "Added to Favorites" and disable the button)
        // show another button to remove from favorites, which will remove the movie from the db and refresh the list of movies to show that the movie has been removed from favorites
        // when searching for movies, if the movie is already in the db, show a message that the movie is already in favorites and disable the button to add to favorites
        // check the movie external id against the list of movies in the db to determine if the movie is already in favorites


        public async Task SearchMovies()
        {
            var searchResults = await MovieLibraryAPI.SearchMoviesAsync(MovieSearch.Title);

            MovieSearchList.Clear();

            if (searchResults != null)
            {
                foreach (var result in searchResults)
                {
                    var movieSearchEntity = new MovieSearchEntity
                    {
                        Title = result.Title!,
                        ReleaseDate = result.ReleaseDate.HasValue
                                    ? DateTime.SpecifyKind(result.ReleaseDate.Value, DateTimeKind.Utc)
                                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                        Overview = result.Overview!,
                        Id = result.Id
                    };

                    MovieSearchList.Add(movieSearchEntity);
                }
            }
        }

        public async Task<List<MovieEntity>> GetMovies()
        {
            return MovieList = await MovieService.GetMoviesAsync();
        }

        public async Task SubmitMovie()
        {
            await MovieService.AddMovieAsync(Movie);
            await GetMovies();
        }

        public async Task AddFavoriteMovieToUser(MovieSearchEntity movie)
        {
            var existingMovie =
                await MovieService.GetByExternalIdAsync(movie.Id);

            if (existingMovie == null)
            {
                var newMovie = new MovieEntity(
                    movie.Id,
                    movie.Title,
                    movie.Overview,
                    movie.ReleaseDate);

                await MovieService.AddMovieAsync(newMovie);

                await MovieService.AddFavoriteMovieAsync(
    "TestUser",
    newMovie);
            }


        }

    }
}