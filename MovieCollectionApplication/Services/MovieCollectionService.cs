using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;
using MovieCollectionInfrastructure.Repository;

namespace MovieCollectionApplication.Services
{
    public class MovieCollectionService : IMovieCollectionService
    {
        private readonly MovieCollectionRepository _repo;
        private readonly ToastService _toastService;

        public MovieCollectionService(MovieCollectionRepository repo, ToastService toastService)
        {
            _repo = repo;
            _toastService = toastService;
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

        public async Task AddFavoriteMovieAsync(Guid userId, MovieEntity movie)
        {
            await _repo.AddFavoriteMovieAsync(userId, movie);
            _toastService.Show("Movie added to favorites!");
        }

        public async Task RemoveFavoriteMovieAsync(Guid userId, int externalId)
        {
            await _repo.RemoveFavoriteMovieAsync(userId, externalId);
            _toastService.Show("Movie removed from favorites!");
        }

        public async Task<List<MovieEntity>> GetFavoriteMoviesAsync(Guid userId, int page, int pageSize)
        {
            return await _repo.GetFavoriteMoviesAsync(userId, page, pageSize);
        }

        public async Task<int> GetTotalFavoriteMoviesAsync(Guid userId)
        {
            return await _repo.GetFavoriteMovieCountAsync(userId);
        }
    }
}