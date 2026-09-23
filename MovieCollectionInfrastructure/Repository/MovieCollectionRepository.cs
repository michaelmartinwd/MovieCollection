using Microsoft.EntityFrameworkCore;
using MovieCollectionEntities;

namespace MovieCollectionInfrastructure.Repository
{
    public class MovieCollectionRepository
    {
        private readonly IDbContextFactory<MovieCollectionDbContext> _repo;

        public MovieCollectionRepository(IDbContextFactory<MovieCollectionDbContext> repo)
        {
            _repo = repo;
        }

        public async Task AddFavoriteMovieAsync(
    string username,
    MovieEntity movie)
        {
            using var context = _repo.CreateDbContext();

            var user = await context.Users
                .Include(u => u.FavoriteMovies)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return;

            //var existingMovie = await context.Movies
            //    .FirstOrDefaultAsync(m => m.ExternalId == movie.ExternalId);

            //if (existingMovie == null)
            //{
            //    existingMovie = new MovieEntity(
            //        movie.ExternalId,
            //        movie.Title,
            //        movie.Overview,
            //        movie.ReleaseDate);

            //    context.Movies.Add(existingMovie);
            //}

            if (!user.FavoriteMovies.Any(m => m.ExternalId == movie.ExternalId))
            {
                user.FavoriteMovies.Add(movie);
            }

            await context.SaveChangesAsync();
        }

        public async Task<UserEntity> AddUser(UserEntity user)
        {
            using var context = _repo.CreateDbContext();
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity?> GetUserAsync(string username)
        {
            using var context = _repo.CreateDbContext();
            return await context.Users
                .Include(u => u.FavoriteMovies)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserEntity?> GetUser(string username)
        {
            using var context = _repo.CreateDbContext();
            return await context.Users.Include(u => u.FavoriteMovies).FirstOrDefaultAsync(m => m.Username == username);
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            using var context = _repo.CreateDbContext();
            return await context.Users.ToListAsync();
        }

        public async Task UpdateUser(UserEntity user)
        {
            using var context = _repo.CreateDbContext();
            //context.Users.Update(user);
            await context.SaveChangesAsync();
           // return user;
        }


        public async Task<MovieEntity?> GetMovie(int externalId)
        {
            using var context = _repo.CreateDbContext();

            return await context.Movies.FirstOrDefaultAsync(m => m.ExternalId == externalId);
        }

        public async Task<List<MovieEntity>> GetAllMovies()
        {
            using var context = _repo.CreateDbContext();

            return await context.Movies.ToListAsync();
        }

        public async Task AddMovie(MovieEntity movieEntity)
        {
            using var context = _repo.CreateDbContext();

            await context.AddAsync(movieEntity);

            await context.SaveChangesAsync();
        }

        public async Task<MovieEntity?> GetByExternalIdAsync(int externalMovieId)
        {
            using var context = _repo.CreateDbContext();

            return await context.Movies
                .FirstOrDefaultAsync(m => m.ExternalId == externalMovieId);
        }
    }
}