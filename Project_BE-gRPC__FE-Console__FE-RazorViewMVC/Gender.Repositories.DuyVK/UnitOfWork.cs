using Gender.Repositories.DuyVK.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gender.Repositories.DuyVK
{
    public interface IUnitOfWork
    {
        ISystemUserAccountRepository SystemUserAccountRepository { get; }
        IMenstrualCycleReminderDuyVKRepository MenstrualCycleReminderDuyVKRepository { get; }
        IReminderCategoryDuyVKRepository ReminderCategoryDuyVKRepository { get; }

        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        // ===============================
        // === Fields & Props
        // ===============================

        private readonly GenderContext _context;

        // ===============================
        // === Constructors
        // ===============================

        public UnitOfWork(
            GenderContext context,
            ISystemUserAccountRepository userRepo,
            IMenstrualCycleReminderDuyVKRepository menstrualRepo,
            IReminderCategoryDuyVKRepository categoryRepo)
        {
            _context = context;
            SystemUserAccountRepository = userRepo;
            MenstrualCycleReminderDuyVKRepository = menstrualRepo;
            ReminderCategoryDuyVKRepository = categoryRepo;
        }

        public ISystemUserAccountRepository SystemUserAccountRepository { get; }

        public IMenstrualCycleReminderDuyVKRepository MenstrualCycleReminderDuyVKRepository { get; }

        public IReminderCategoryDuyVKRepository ReminderCategoryDuyVKRepository { get; }


        // ===============================
        // === Methods
        // ===============================

        // DI system will automatically handles lifetimes
        // You dont need to dispose it here, which might cause problems
        //public async ValueTask DisposeAsync()
        //{
        //    //await _context.DisposeAsync();
        //}

        // ===============================
        // === Methods
        // ===============================

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }


    }

}
