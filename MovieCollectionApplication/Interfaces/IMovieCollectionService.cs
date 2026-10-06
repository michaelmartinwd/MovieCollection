using MovieCollectionEntities;
using static MovieCollectionInfrastructure.Repository.MovieCollectionRepository;

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
        Task<PagedResult<MovieEntity>> GetFavoriteMoviesAsync(Guid userId, string? title, int? year, int page, int pageSize);
        Task<int> GetTotalFavoriteMoviesAsync(Guid userId);
    }
}