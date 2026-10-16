using MovieCollectionEntities;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace MovieCollectionApplication.Services
{
    public class CurrentUserService
    {
        private readonly UserService _userService;
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserEntity? CurrentUser { get; private set; }

        public CurrentUserService(
       UserService userService,
       AuthenticationStateProvider authStateProvider)
        {
            _userService = userService;
            _authStateProvider = authStateProvider;
        }

        public async Task<UserEntity?> GetCurrentUserAsync()
        {
            if (CurrentUser is not null)
                return CurrentUser;

            var authState =
                await _authStateProvider.GetAuthenticationStateAsync();

            var userIdValue = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
                return null;

            CurrentUser = await _userService.GetUserByIdAsync(userId);

            return CurrentUser;
        }
    
}
}