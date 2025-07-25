using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Services.DuyVK.Interfaces
{
    public interface ISystemUserAccountService
    {
        public Task<SystemUserAccount?> GetUserAccountAsync(string username, string password);

        public string GenerateJSONWebToken(SystemUserAccount systemUserAccount);
    }
}
