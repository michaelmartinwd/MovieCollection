using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class MovieEntity
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public DateTime DateAdded { get; set;  }

        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();

        public MovieEntity()
        {
        }

        public MovieEntity(int externalId, string title, string overview, DateTime releaseDate)
        {
            Id = Guid.NewGuid();
            ExternalId = externalId;
            Title = title;
            Overview = overview;
            ReleaseDate = releaseDate;
            DateAdded = DateTime.UtcNow;
        }

    }
}