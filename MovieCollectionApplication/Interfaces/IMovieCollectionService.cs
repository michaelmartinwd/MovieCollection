using MovieCollectionEntities;

namespace MovieCollectionApplication.Interfaces
{
    public interface IMovieCollectionService
    {
        Task AddMovieAsync(MovieEntity movie);
        Task<MovieEntity> GetMovieAsync(int externalId);
        Task<List<MovieEntity>> GetMoviesAsync();

        Task<MovieEntity?> GetByExternalIdAsync(int externalMovieId);
        Task AddFavoriteMovieAsync(Guid userId, MovieEntity movie);
        Task RemoveFavoriteMovieAsync(Guid userId, int externalId);
        Task<List<MovieEntity>> GetFavoriteMoviesAsync(Guid userId, int page, int pageSize);
        Task<int> GetTotalFavoriteMoviesAsync(Guid userId);
    }
}