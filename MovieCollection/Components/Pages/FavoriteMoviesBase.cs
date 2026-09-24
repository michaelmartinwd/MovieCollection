using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class FavoriteMoviesBase : ComponentBase
    {

        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMovieCollectionService MovieService { get; set; } = null!;

        public ICollection<MovieEntity>? FavoriteMovies { get; set; }

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            var user = await UserService.GetUserAsync("TestUser");
            FavoriteMovies = user!.FavoriteMovies;
            StateHasChanged();
        }

        public async Task RemoveFromFavorites(MovieEntity movie)
        {
         
            var user = await UserService.GetUserAsync("TestUser");
            if (user != null && user.FavoriteMovies.Any(x => x.ExternalId == movie.ExternalId))
            {
                await MovieService.RemoveFavoriteMovieAsync(user.Username, movie.ExternalId);
                FavoriteMovies!.Remove(movie);
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