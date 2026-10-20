using MovieCollectionEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieCollectionInfrastructure.Interfaces
{
    public interface IMovieCollectionRepository
    {
        Task AddFavoriteMovieAsync(Guid userId, MovieEntity movie);
        Task RemoveFavoriteMovieAsync(Guid userId, int movieId);

        Task<UserEntity> AddUser(UserEntity user);
        Task<UserEntity?> GetUserAsync(string username);
        Task<UserEntity?> GetUser(string username);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<List<UserEntity>> GetAllUsers();
        Task UpdateUser(UserEntity user);

        Task<MovieEntity?> GetMovie(int externalId);
        Task<List<MovieEntity>> GetAllMovies();
        Task AddMovie(MovieEntity movieEntity);
        Task<MovieEntity?> GetByExternalIdAsync(int externalMovieId);

        Task<PagedResult<MovieEntity>> GetFavoriteMoviesAsync(Guid userId, string? title, int? year, int page, int pageSize);
        Task<int> GetFavoriteMovieCountAsync(Guid userId);
        Task DeleteExpiredDemoUsersAsync();
    }
}
