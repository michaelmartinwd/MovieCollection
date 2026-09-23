using Microsoft.EntityFrameworkCore;
using MovieCollectionEntities;


namespace MovieCollectionInfrastructure
{
   
    public class MovieCollectionDbContext : DbContext
    {
        public MovieCollectionDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieCollectionDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
