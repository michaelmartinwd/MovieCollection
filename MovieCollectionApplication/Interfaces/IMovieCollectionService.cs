using MovieCollectionEntities;

namespace MovieCollectionApplication.Interfaces
{
    public interface IMovieCollectionService
    {
        Task AddMovieAsync(MovieEntity movie);
        Task UpdateMovieAsync(MovieEntity movie);
        Task DeleteMovieAsync(Guid id);
        Task<MovieEntity> GetMovieAsync(string title);
        Task<List<MovieEntity>> GetMoviesAsync();
    }
}