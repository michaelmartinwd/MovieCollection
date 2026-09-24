using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;
using MovieCollectionInfrastructure.Repository;

namespace MovieCollectionApplication.Services
{
    public class MovieCollectionService : IMovieCollectionService
    {
        private readonly MovieCollectionRepository _repo;

        public MovieCollectionService(MovieCollectionRepository repo)
        {
            _repo = repo;
        }

        public async Task AddMovieAsync(MovieEntity movie)
        {
            var movieEntity = new MovieEntity(movie.ExternalId, movie.Title, movie.Overview, movie.ReleaseDate);

           await _repo.AddMovie(movieEntity);
        }

        public async Task<MovieEntity> GetMovieAsync(int externalId)
        {
            return await _repo.GetMovie(externalId);
        }

        public async Task<List<MovieEntity>> GetMoviesAsync()
        {
            return await _repo.GetAllMovies();
        }

        public async Task<MovieEntity?> GetByExternalIdAsync(int externalMovieId)
        {
            return await _repo.GetByExternalIdAsync(externalMovieId);
        }

        public async Task AddFavoriteMovieAsync(string username, MovieEntity movie)
        {
            await _repo.AddFavoriteMovieAsync(username, movie);
        }

        public async Task RemoveFavoriteMovieAsync(string username, int externalId)
        {
            await _repo.RemoveFavoriteMovieAsync(username, externalId);
        }
    }
}