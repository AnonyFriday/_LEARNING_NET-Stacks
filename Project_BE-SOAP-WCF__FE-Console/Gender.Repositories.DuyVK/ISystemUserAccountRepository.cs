using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.Models;

namespace Gender.Repositories.DuyVK
{
    public interface ISystemUserAccountRepository : IGenericRepository<SystemUserAccount>
    {
        Task<SystemUserAccount?> GetAsync(string username, string password);
    }
}