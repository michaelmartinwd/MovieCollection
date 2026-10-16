using Microsoft.AspNetCore.Components;
using MovieCollectionAPI;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class MoviesBase : ComponentBase, IDisposable
    {
        [Inject]
        private CurrentUserService CurrentUserService { get; set; } = null!;
        [Inject]
        private ToastService ToastService { get; set; } = null!;
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;
        [Inject]
        private MovieLibraryAPI MovieLibraryAPI { get; set; } = null!;

        public List<MovieSearchEntity> MovieSearchList { get; set; } = [];
        public MovieSearchEntity MovieSearch { get; set; } = new();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string SearchTitle { get; set; } = string.Empty;
        public UserEntity CurrentUser { get; set; } = new();

        public bool ShowToast { get; set; }
        public string ToastMessage { get; set; } = string.Empty;

        public ICollection<MovieEntity> FavoriteMovies { get; set; } = [];

        public HashSet<int> ExpandedMovies = [];

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ToastService.OnShow += ShowToastHandler;

            await SearchMovies();

            var currentUser = await CurrentUserService.GetCurrentUserAsync();
            CurrentUser = currentUser ?? new UserEntity();

            if (CurrentUser is not null)
            { 
                FavoriteMovies = CurrentUser.FavoriteMovies ?? new List<MovieEntity>();
            }
            else
            {
                FavoriteMovies = new List<MovieEntity>();
            }
        }

        private void ShowToastHandler(string message)
        {
            ToastMessage = message;
            ShowToast = true;
            InvokeAsync(StateHasChanged);
        }

        public void HideToast()
        {
            ShowToast = false;
        }

        public void Dispose()
        {
            ToastService.OnShow -= ShowToastHandler;
        }

        public async Task SearchMovies()
        {
            SearchTitle = MovieSearch.Title ?? string.Empty;

            var searchResults = await MovieLibraryAPI.SearchMoviesAsync(SearchTitle, CurrentPage);

            MovieSearchList.Clear();

            if (searchResults != null)
            {
                TotalPages = searchResults.TotalPages;
                CurrentPage = searchResults.Page;

                if (searchResults.Results != null && searchResults.Results.Count > 0)
                {
                    foreach (var result in searchResults.Results)
                    {
                        var movieSearchEntity = new MovieSearchEntity
                        {
                            Title = result.Title ?? string.Empty,
                            ReleaseDate = result.ReleaseDate.HasValue
                                ? DateTime.SpecifyKind(result.ReleaseDate.Value, DateTimeKind.Utc)
                                : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                            Overview = result.Overview ?? string.Empty,
                            Id = result.Id
                        };

                        MovieSearchList.Add(movieSearchEntity);
                    }
                }

                MovieSearchList = MovieSearchList.OrderBy(m => m.Title).ToList();
            }
        }

        public async Task LoadPage(int currentPage)
        {
            CurrentPage = currentPage;
            await SearchMovies();
        }

        public async Task AddFavoriteMovieToUser(MovieSearchEntity movie)
        {
            if (movie == null) return;

            var existingMovie = await MovieService.GetByExternalIdAsync(movie.Id);

            if (existingMovie == null)
            {
                var newMovie = new MovieEntity(
                    movie.Id,
                    movie.Title,
                    movie.Overview,
                    movie.ReleaseDate);

                await MovieService.AddMovieAsync(newMovie);
                await MovieService.AddFavoriteMovieAsync(CurrentUser.Id, newMovie);
            }
            else
            {
                // Only add if not already a favorite
                if (CurrentUser?.FavoriteMovies == null || !CurrentUser.FavoriteMovies.Any(x => x.ExternalId == movie.Id))
                {
                    await MovieService.AddFavoriteMovieAsync(CurrentUser!.Id, existingMovie);
                }
            }

            var user = await UserService.GetUserByIdAsync(CurrentUser.Id);
            CurrentUser = user!;
            FavoriteMovies = CurrentUser?.FavoriteMovies!;
            await InvokeAsync(StateHasChanged);
        }

        public void ToggleText(int movieId)
        {
            if (!ExpandedMovies.Add(movieId))
            {
                ExpandedMovies.Remove(movieId);
            }
        }
    }
}