using Microsoft.EntityFrameworkCore;
using MovieCollectionEntities;

namespace MovieCollectionInfrastructure.Repository
{
    public class MovieCollectionRepository
    {
        //private MovieCollectionDbContext _dbContext;
        private readonly IDbContextFactory<MovieCollectionDbContext> _repo;

        //public MovieCollectionRepository(MovieCollectionDbContext context) 
        //{
        //    _dbContext = context;
        //}

        public MovieCollectionRepository(IDbContextFactory<MovieCollectionDbContext> repo)
        {
            _repo = repo;
        }

        public async Task<MovieEntity?> GetMovie(string title)
        {
            using var context = _repo.CreateDbContext();

            return await context.Movies.FirstOrDefaultAsync(m => m.Title == title);
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
    }
}