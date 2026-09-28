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
        private ToastService ToastService { get; set; } = null!;
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;
        [Inject]
        private MovieLibraryAPI MovieLibraryAPI { get; set; } = null!;

        //public MovieEntity Movie { get; set; } = new();

        //public List<MovieEntity> MovieList { get; set; } = new();
        public List<MovieSearchEntity> MovieSearchList { get; set; } = new();
        public MovieSearchEntity MovieSearch { get; set; } = new();

        public int CurrentPage = 1;
        public int TotalPages;
        public string SearchTitle = "";
        public UserEntity CurrentUser { get; set; } = new();

        public bool showToast;
        public string toastMessage = "";

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            ToastService.OnShow += ShowToast;
            await SearchMovies();
            CurrentUser = await UserService.GetUserAsync("TestUser");
            StateHasChanged();
        }

        public void ShowToast(string message)
        {
            toastMessage = message;
            showToast = true;

            InvokeAsync(StateHasChanged);
        }

        public void HideToast()
        {
            showToast = false;
        }

        public void Dispose()
        {
            ToastService.OnShow -= ShowToast;
        }

        public async Task SearchMovies()
        {
            SearchTitle = MovieSearch.Title ?? "";

            var searchResults = await MovieLibraryAPI.SearchMoviesAsync(SearchTitle, CurrentPage);

            MovieSearchList.Clear();

            if (searchResults != null)
            {
                TotalPages = searchResults.TotalPages;
                CurrentPage = searchResults.Page;

                if (searchResults.Results != null && searchResults.Results.Count > 0)
                {
                    foreach (var result in searchResults.Results!)
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
        }

        public async Task LoadPage(int currentPage)
        {
            CurrentPage = currentPage;

            await SearchMovies();
        }

        //public async Task<List<MovieEntity>> GetMovies()
        //{
        //    return MovieList = await MovieService.GetMoviesAsync();
        //}

        //public async Task SubmitMovie()
        //{
        //    await MovieService.AddMovieAsync(Movie);
        //    await GetMovies();
        //}

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
            else if (existingMovie is not null && CurrentUser is not null && CurrentUser.FavoriteMovies.Any(x => x.ExternalId != movie.Id))
            {
                await MovieService.AddFavoriteMovieAsync(
"TestUser",
existingMovie);
            }


        }

    }
}