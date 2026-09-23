using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;    
        public ICollection<MovieEntity> FavoriteMovies { get; set; } = new List<MovieEntity>();

        public UserEntity() {}

        public UserEntity(Guid id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}