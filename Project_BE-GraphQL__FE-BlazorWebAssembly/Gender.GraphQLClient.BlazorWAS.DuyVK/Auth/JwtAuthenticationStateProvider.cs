using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        public readonly ILocalStorageService _localStorageService;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        /// <summary>
        /// Get the token from the local storage
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTokenFromStorage()
        {
            // 1. Read the token from local storage
            var token = await _localStorageService.GetItemAsync<string>(AppCts.LocalStorage.TokenKey);
            // 2. If no token, return null
            if (token == null)
            {
                return null;
            }
            // 3. If token exists, return it
            return token;
        }

        /// <summary>
        /// Get the current state of the framework
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // 1. Read the token first
            var token = await _localStorageService.GetItemAsync<string>(AppCts.LocalStorage.TokenKey);

            // 2. If no token, then create the anonymous claims principal
            if (token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // 3. If token, parse the token, create the claims principal and return it
            var jwtContent = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtContent.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Call after the successfull login
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task MarkUserAsAuthenticated(string token)
        {
            // 1. Set token to the locla storage
            await _localStorageService.SetItemAsync(AppCts.LocalStorage.TokenKey, token);

            // 2. Parsing the token and creating ClaimsPrincipal
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            // 3. Tell Blazor that the authentication state has changed
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user))
            );
        }

        /// <summary>
        /// Logout then notify the current claims principal as anonymous
        /// </summary>
        /// <returns></returns>
        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync(AppCts.LocalStorage.TokenKey);
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            );
        }
    }
}
