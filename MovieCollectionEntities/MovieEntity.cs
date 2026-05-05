using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class MovieEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime DateAdded { get; set;  }
        public DateTime DateUpdated { get; set;  }

        public MovieEntity()
        {
        }

        public MovieEntity(string title, string description, string genre)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Genre = genre;
            DateAdded = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
        }

        public void UpdateMovie(string title, string description, string genre)
        {
            Title = title;
            Description = description;
            Genre = genre;
            DateUpdated = DateTime.UtcNow;
        }
    }
}