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

        public ICollection<MovieEntity>? FavoriteMovies { get; set; }

        public MovieEntity? SelectedMovie { get; set; }

        public bool showToast;
        public string toastMessage = "";

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            ToastService.OnShow += ShowToast;
            var user = await UserService.GetUserAsync("TestUser");
            FavoriteMovies = user!.FavoriteMovies;
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
                await MovieService.RemoveFavoriteMovieAsync(user.Username, movie.ExternalId);
                FavoriteMovies!.Remove(movie);
                CloseModal();
                StateHasChanged();
            }
        }
            //}
            // 2. Filter favorite movies by release year, title
            //public void FilterFavoriteMovies(string? title, int? releaseYear)
            //{
            //    if (FavoriteMovies == null)
            //        return;
            //    var filteredMovies = FavoriteMovies.AsQueryable();
            //    if (!string.IsNullOrEmpty(title))
            //    {
            //        filteredMovies = filteredMovies.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            //    }
            //    if (releaseYear.HasValue)
            //    {
            //        filteredMovies = filteredMovies.Where(m => m.ReleaseYear == releaseYear.Value);
            //    }
            //    FavoriteMovies = filteredMovies.ToList();
            //    StateHasChanged();
            //}
        }
}