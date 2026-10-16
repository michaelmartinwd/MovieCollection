using MovieCollectionEntities;

namespace MovieCollectionApplication.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> AddUserAsync(UserEntity user);
        Task<UserEntity?> GetUserAsync(string username);
        Task UpdateUser(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<UserEntity> CreateDemoUserAsync();
    }
}