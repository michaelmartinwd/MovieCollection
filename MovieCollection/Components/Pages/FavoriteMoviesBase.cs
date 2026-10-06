using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class FavoriteMoviesBase : ComponentBase
    {
        [Inject]
        private ToastService ToastService { get; set; } = null!;
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;

        public ICollection<MovieEntity> FavoriteMovies { get; set; }
        public List<MovieEntity> PagedMovies { get; set; } = new();

        public MovieEntity? SelectedMovie { get; set; }
        public UserEntity User { get; set; } = new();

        public bool showToast;

        public string toastMessage = "";


        public int CurrentPage = 1;
        public int PageSize = 10;
        public int TotalMovies;
        public int TotalPages;

        public string SearchTitle { get; set; } = string.Empty;
        public int? SearchYear { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            ToastService.OnShow += ShowToast;
            User = await UserService.GetUserAsync("TestUser");

            if (User is not null)
            {
                FavoriteMovies = await MovieService.GetFavoriteMoviesAsync(User.Id, 1, 10);
                await LoadPage(1);
            }
        }

        public async Task LoadPage(int page)
        {
            CurrentPage = page;

            var movies = FavoriteMovies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTitle))
            {
                movies = movies.Where(m =>
                    m.Title.Contains(
                        SearchTitle,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (SearchYear.HasValue)
            {
                movies = movies.Where(m =>
                    m.ReleaseDate.Year == SearchYear.Value);
            }

            TotalMovies = movies.Count();
            TotalPages = (int)Math.Ceiling(TotalMovies / (double)PageSize);

            PagedMovies = movies
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
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

        public bool ShowModal = false;

        public void OpenModal(MovieEntity movie)
        {
            SelectedMovie = movie;
            ShowModal = true;
        }

        public void CloseModal()
        {
            ShowModal = false;
        }

        public async Task RemoveFromFavorites(MovieEntity movie)
        {
            var user = await UserService.GetUserAsync("TestUser");
            if (user != null && user.FavoriteMovies.Any(x => x.ExternalId == movie.ExternalId))
            {
                await MovieService.RemoveFavoriteMovieAsync(user.Id, movie.ExternalId);
                FavoriteMovies!.Remove(movie);
                CloseModal();
                StateHasChanged();
            }
        }

        public void ClearFavoriteMovieFilters()
        {
            SearchTitle = string.Empty;
            SearchYear = null;
            CurrentPage = 1;

            TotalMovies = FavoriteMovies.Count;
            TotalPages = (int)Math.Ceiling(TotalMovies / (double)PageSize);

            PagedMovies = FavoriteMovies
                .Take(PageSize)
                .ToList();
        }

    }
}