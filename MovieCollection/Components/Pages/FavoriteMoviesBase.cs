using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class FavoriteMoviesBase : ComponentBase, IDisposable
    {
        [Inject]
        private ToastService ToastService { get; set; } = null!;
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;

        public ICollection<MovieEntity> FavoriteMovies { get; set; } = new List<MovieEntity>();
        public List<MovieEntity> PagedMovies { get; set; } = new();

        public MovieEntity? SelectedMovie { get; set; }
        public UserEntity? User { get; set; }

        public bool ShowModal { get; set; }
        public bool IsToastVisible { get; set; }
        public string ToastMessage { get; set; } = string.Empty;

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalMovies { get; set; }
        public int TotalPages { get; set; }

        public string SearchTitle { get; set; } = string.Empty;
        public int? SearchYear { get; set; }

        public HashSet<int> ExpandedMovies = [];

        protected override async Task OnInitializedAsync()
        {
            ToastService.OnShow += ShowToast;
            User = await UserService.GetUserAsync("TestUser");

            if (User is not null)
            {
                await LoadPage(1);
            }
        }

        public async Task LoadPage(int page)
        {
            if (User is null) return;

            CurrentPage = page;

            var (items, total) = await MovieService.GetFavoriteMoviesAsync(
                User.Id, SearchTitle, SearchYear, CurrentPage, PageSize);

            PagedMovies = items ?? new List<MovieEntity>();
            TotalMovies = total;
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        }

        public void ShowToast(string message)
        {
            ToastMessage = message;
            IsToastVisible = true;
            InvokeAsync(StateHasChanged);
        }

        public void HideToast()
        {
            IsToastVisible = false;
        }

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
            if (user is null) return;

            if (user.FavoriteMovies.Any(x => x.ExternalId == movie.ExternalId))
            {
                await MovieService.RemoveFavoriteMovieAsync(user.Id, movie.ExternalId);
                CloseModal();
                await LoadPage(CurrentPage);
                StateHasChanged();
            }
        }

        public async Task ClearFavoriteMovieFilters()
        {
            SearchTitle = string.Empty;
            SearchYear = null;

            await LoadPage(1);
        }

        public async Task ConfirmRemove()
        {
            if (SelectedMovie is not null)
            {
                await RemoveFromFavorites(SelectedMovie);
            }
        }

        public void ToggleText(int movieId)
        {
            if (!ExpandedMovies.Add(movieId))
            {
                ExpandedMovies.Remove(movieId);
            }
        }

        public void Dispose()
        {
            if (ToastService != null)
            {
                ToastService.OnShow -= ShowToast;
            }
        }
    }
}