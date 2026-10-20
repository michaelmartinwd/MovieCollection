using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MovieCollectionEntities;
using MovieCollectionInfrastructure.Interfaces;

namespace MovieCollectionInfrastructure.Repository
{
    public class MovieCollectionRepository : IMovieCollectionRepository
    {
        private readonly IDbContextFactory<MovieCollectionDbContext> _repo;

        public MovieCollectionRepository(IDbContextFactory<MovieCollectionDbContext> repo)
        {
            _repo = repo;
        }

        public async Task AddFavoriteMovieAsync(Guid userId, MovieEntity movie)
        {
            using var context = await _repo.CreateDbContextAsync();

            var user = await context.Users
                .Include(u => u.FavoriteMovies)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return;

            var existingMovie = await context.Movies
                .FirstOrDefaultAsync(m => m.ExternalId == movie.ExternalId);

            var movieToAdd = existingMovie ?? movie;

            if (existingMovie == null)
            {
                await context.Movies.AddAsync(movieToAdd);
            }

            var alreadyFavorite = user.FavoriteMovies.Any(m => m.ExternalId == movie.ExternalId);

            if (!alreadyFavorite)
            {
                user.FavoriteMovies.Add(movieToAdd);
                await context.SaveChangesAsync();
            }
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

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            using var context = _repo.CreateDbContext();
            return await context.Users
                .Include(u => u.FavoriteMovies)
                .FirstOrDefaultAsync(u => u.Id == userId);
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
            await using var context = await _repo.CreateDbContextAsync();
            context.Users.Update(user);
            await context.SaveChangesAsync();
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

        public async Task RemoveFavoriteMovieAsync(Guid userId, int movieId)
        {
            await using var context = await _repo.CreateDbContextAsync();

            var user = await context.Users
                .Include(u => u.FavoriteMovies)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return;

            var movie = user.FavoriteMovies
                .FirstOrDefault(m => m.ExternalId == movieId);

            if (movie == null)
                return;

            user.FavoriteMovies.Remove(movie);

            await context.SaveChangesAsync();
        }

        public async Task<PagedResult<MovieEntity>> GetFavoriteMoviesAsync(
            Guid userId, string? title, int? year, int page, int pageSize)
        {
            await using var context = await _repo.CreateDbContextAsync();

            var query = context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.FavoriteMovies)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(title))
            {
                var t = title.Trim().ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(t));
            }

            if (year.HasValue)
                query = query.Where(m => m.ReleaseDate.Year == year.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MovieEntity>(items, total);
        }

        public async Task<int> GetFavoriteMovieCountAsync(Guid userId)
        {
            await using var context = await _repo.CreateDbContextAsync();

            return await context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.FavoriteMovies)
                .CountAsync();
        }

        public async Task DeleteExpiredDemoUsersAsync()
        {
            await using var context = await _repo.CreateDbContextAsync();

            var expirationDate = DateTime.UtcNow.AddDays(-2);

            var expiredUsers = await context.Users
                .Where(u => u.DateCreated < expirationDate)
                .ToListAsync();

            context.Users.RemoveRange(expiredUsers);

            await context.SaveChangesAsync();
        }
    }
}