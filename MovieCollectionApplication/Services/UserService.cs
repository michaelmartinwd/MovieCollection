using MovieCollectionApplication.Interfaces;
using MovieCollectionEntities;
using MovieCollectionInfrastructure.Repository;

namespace MovieCollectionApplication.Services
{
    public class UserService : IUserService
    {
        private readonly MovieCollectionRepository _repo;

        public UserService(MovieCollectionRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user)
        {
            var userEntity = new UserEntity(user.Id, user.Username);
            var newUser = await _repo.AddUser(userEntity);
            return newUser;
        }

        public Task<UserEntity> GetUserAsync(string username)
        {
            return _repo.GetUser(username);
        }

        public Task UpdateUser(UserEntity user)
        {
            return _repo.UpdateUser(user);
        }
    }
}