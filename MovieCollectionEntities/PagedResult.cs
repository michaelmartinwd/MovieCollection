using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionEntities
{
    public record PagedResult<T>(List<T> Items, int Total);
}