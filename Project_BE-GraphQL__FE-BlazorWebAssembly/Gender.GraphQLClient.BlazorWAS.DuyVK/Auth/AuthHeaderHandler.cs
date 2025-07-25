using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace Gender.GraphQLClient.BlazorWAS.DuyVK.Auth
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _tokenCookieName;
        private readonly JwtAuthenticationStateProvider _storageProvider;

        public AuthHeaderHandler(IJSRuntime jsRuntime, JwtAuthenticationStateProvider storageProvider)
        {
            _jsRuntime = jsRuntime;
            _tokenCookieName = AppCts.LocalStorage.TokenKey;
            _storageProvider = storageProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get the token from the browser cookie using JS interop
            var token = await _storageProvider.GetTokenFromStorage();

            // Add the token to the Authorization header if found
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Continue the request pipeline
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
