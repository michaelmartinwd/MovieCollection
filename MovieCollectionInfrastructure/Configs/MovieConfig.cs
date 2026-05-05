using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCollectionEntities;


namespace MovieCollectionInfrastructure.Configs
{
    public class MovieConfig : IEntityTypeConfiguration<MovieEntity>
    {
        public void Configure(EntityTypeBuilder<MovieEntity> builder)
        {
            builder.ToTable("Movies");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property("Title");
            builder.Property("Description");
            builder.Property("Genre");
            builder.Property("DateAdded");
            builder.Property("DateUpdated");
        }
    }
}