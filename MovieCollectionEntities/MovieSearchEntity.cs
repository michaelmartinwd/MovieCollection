using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public class MovieSearchEntity
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Overview { get; set; }
        public int Id { get; set; }
    }
}