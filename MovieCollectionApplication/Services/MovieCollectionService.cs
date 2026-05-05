using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;
using MovieCollectionInfrastructure.Repository;

namespace MovieCollectionApplication.Services
{
    public class MovieCollectionService(MovieCollectionRepository repo) : IMovieCollectionService
    {
        private readonly MovieCollectionRepository _repo = repo;

        public async Task AddMovieAsync(MovieEntity movie)
        {
            var movieEntity = new MovieEntity(movie.Title, movie.Description, movie.Genre);

           await _repo.AddMovie(movieEntity);
        }

        public async Task UpdateMovieAsync(MovieEntity movie)
        {
            // Implementation to update a movie in the collection
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            // Implementation to delete a movie from the collection
        }

        public async Task<MovieEntity> GetMovieAsync(string title)
        {
            return await _repo.GetMovie(title);
        }

        public async Task<List<MovieEntity>> GetMoviesAsync()
        {
            return await _repo.GetAllMovies();
        }
    }
}