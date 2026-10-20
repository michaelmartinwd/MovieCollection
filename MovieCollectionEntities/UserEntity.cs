using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty; 
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public ICollection<MovieEntity> FavoriteMovies { get; set; } = new List<MovieEntity>();

        public UserEntity() {}

        public UserEntity(Guid id, string username, DateTime dateCreated)
        {
            Id = id;
            Username = username;
            DateCreated = dateCreated;
        }
    }
}