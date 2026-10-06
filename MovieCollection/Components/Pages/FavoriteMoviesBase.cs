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
        public UserEntity User { get; set; } = new(); 

        public bool showToast;

        public string toastMessage = "";
        public string SearchTitle = "";


        public int CurrentPage = 1;
        public int PageSize = 10;
        public int TotalMovies;
        public int TotalPages;
      
        protected async override void OnInitialized()
        {
            base.OnInitialized();
            ToastService.OnShow += ShowToast;
            User = await UserService.GetUserAsync("TestUser");

            if (User is not null)
            {
                TotalMovies = await MovieService.GetTotalFavoriteMoviesAsync(User.Id);
                TotalPages = (int)Math.Ceiling(TotalMovies / (double)PageSize);

                await LoadPage(1);
            }
        }

        public async Task LoadPage(int page)
        {
            CurrentPage = page;

            FavoriteMovies = await MovieService.GetFavoriteMoviesAsync(
                User.Id,
                CurrentPage,
                PageSize);
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
                await MovieService.RemoveFavoriteMovieAsync(user.Id, movie.ExternalId);
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