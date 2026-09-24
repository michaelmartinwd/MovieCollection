using MovieCollectionEntities;

namespace MovieCollectionApplication.Interfaces
{
    public interface IMovieCollectionService
    {
        Task AddMovieAsync(MovieEntity movie);
        Task<MovieEntity> GetMovieAsync(int externalId);
        Task<List<MovieEntity>> GetMoviesAsync();

        Task<MovieEntity?> GetByExternalIdAsync(int externalMovieId);
        Task AddFavoriteMovieAsync(string username, MovieEntity movie);
        Task RemoveFavoriteMovieAsync(string username, int externalId);
    }
}