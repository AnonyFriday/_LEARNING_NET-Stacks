using Gender.Repositories.DuyVK.Basic;
using Gender.Repositories.DuyVK.DBContext;
using Gender.Repositories.DuyVK.Models;
using Microsoft.EntityFrameworkCore;

namespace Gender.Repositories.DuyVK
{
    public class SystemUserAccountRepository : GenericRepository<SystemUserAccount>, ISystemUserAccountRepository
    {
        // ====================
        // === Constructors
        // ====================

        public SystemUserAccountRepository(GenderContext context) : base(context) { }

        // ====================
        // === Methods
        // ====================

        /// <summary>
        /// Get user by username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SystemUserAccount?> GetAsync(string username, string password)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync
                (u => u.UserName == username && u.Password == password);

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync
            //    (u => u.UserName == username && u.Password == password && u.IsActive == true);

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync
            //    (u => u.Email == username && u.Password == password && u.IsActive == true);

            //return await _context.SystemUserAccounts.FirstOrDefaultAsync
            //    (u => u.Phone == username && u.Password == password && u.IsActive == true);
        }
    }
}
