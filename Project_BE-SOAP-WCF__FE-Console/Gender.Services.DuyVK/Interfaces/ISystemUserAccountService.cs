using Gender.Repositories.DuyVK.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Gender.Services.DuyVK.Interfaces
{
    public interface ISystemUserAccountService
    {
        public Task<SystemUserAccount?> GetUserAccountAsync(string username, string password);

        public string GenerateJSONWebToken(SystemUserAccount systemUserAccount);

        public ClaimsPrincipal? ValidateJSONWebToken(string token);

        public ClaimsPrincipal? AuthorizeRequest(IHttpContextAccessor accessor, params string[] roles);
    }
}
