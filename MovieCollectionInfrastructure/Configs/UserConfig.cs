using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCollectionEntities;

namespace MovieCollectionInfrastructure.Configs
{
    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.DateCreated);

            builder.HasMany(u => u.FavoriteMovies)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFavoriteMovies",
                    j => j.HasOne<MovieEntity>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("UserId", "MovieId");
                        j.ToTable("UserFavoriteMovies");
                    });
        }
    }
}