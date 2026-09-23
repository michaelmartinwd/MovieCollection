using Microsoft.EntityFrameworkCore;
using MovieCollectionApplication.Services;
using MovieCollectionEntities;
using System.Reflection.Metadata.Ecma335;

namespace MovieCollection.SeedData
{
    public class Seed
    {
        public Seed() { }

        public static async Task Initialize(IServiceScope scope)
        {
            await InitalizeUser(scope);
        }

        public static async Task<UserEntity> InitalizeUser(IServiceScope scope)
        {

           var userService = scope.ServiceProvider
                    .GetRequiredService<UserService>();

            var existingUser = await userService.GetUserAsync("TestUser");

            if (existingUser is not null)
                return existingUser;

            var newUser = new UserEntity { Id = Guid.Parse("b12bcbbd-a712-4cbd-83e8-ada17771c7a9"), Username = "TestUser" };

            var user = await userService.AddUserAsync(newUser);

            return user;
        }
    }
}