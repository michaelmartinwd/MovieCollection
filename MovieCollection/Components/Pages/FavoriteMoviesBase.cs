using Microsoft.AspNetCore.Components;
using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;

namespace MovieCollection.Components.Pages
{
    public partial class FavoriteMoviesBase : ComponentBase
    {

        [Inject]
        private IUserService UserService { get; set; } = null!;

        public ICollection<MovieEntity>? FavoriteMovies { get; set; }

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            var user = await UserService.GetUserAsync("TestUser");
            FavoriteMovies = user!.FavoriteMovies;
            StateHasChanged();
        }

        
    }
}