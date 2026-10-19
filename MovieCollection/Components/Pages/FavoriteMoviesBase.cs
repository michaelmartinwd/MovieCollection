using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class FavoriteMoviesBase : ComponentBase
    {
        [Inject]
        private CurrentUserService CurrentUserService { get; set; } = null!;
        [Inject]
        private ToastService ToastService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;

        public List<MovieEntity> PagedMovies { get; set; } = [];
        public MovieEntity? SelectedMovie { get; set; }
        public UserEntity CurrentUser { get; set; } = new();
        public HashSet<int> ExpandedMovies = [];

        public bool ShowModal { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalMovies { get; set; }
        public int TotalPages { get; set; }
        public int? SearchYear { get; set; }

        public string SearchTitle { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            var currentUser = await CurrentUserService.GetCurrentUserAsync();
            CurrentUser = currentUser ?? new UserEntity();

            if (CurrentUserService.CurrentUser is not null)
                await LoadPage(1);
        }

        public async Task LoadPage(int page)
        {
            if (CurrentUser is null) return;

            CurrentPage = page;

            var result = await MovieService.GetFavoriteMoviesAsync(
                CurrentUser.Id, SearchTitle, SearchYear, CurrentPage, PageSize);

            PagedMovies = result.Items ?? new List<MovieEntity>();
            TotalMovies = result.Total;
            TotalPages = (int)Math.Ceiling(TotalMovies / (double)PageSize);
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
            if (CurrentUser is null) return;

            await MovieService.RemoveFavoriteMovieAsync(
                CurrentUser.Id, movie.ExternalId);

            await LoadPage(CurrentPage);

            CloseModal();

            ToastService.ShowSuccess("Movie removed from favorites.");
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
    }
}