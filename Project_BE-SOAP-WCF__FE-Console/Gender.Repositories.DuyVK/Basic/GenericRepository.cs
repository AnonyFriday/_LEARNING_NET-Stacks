using Gender.Repositories.DuyVK.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Gender.Repositories.DuyVK.Basic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // ==========================
        // === Fields
        // ==========================

        protected readonly GenderContext _context;

        // ==========================
        // === Constructors
        // ==========================

        public GenericRepository(GenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ==========================
        // === GET
        // ==========================

        virtual public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        virtual public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        /*
        https://guidgenerator.com/
        uniqueidentifier | guid: daacb4fb-ff73-46ef-98f1-4af9aab2a30a
         */
        public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        // ==========================
        // === Create
        // ==========================

        virtual public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        // ==========================
        // === Update
        // ==========================

        public void Update(T entity)
        {
            // 1. Turn off tracking (optional, explained below)
            _context.ChangeTracker.Clear();

            // 2. Attach the entity to the context
            var tracker = _context.Attach(entity);

            // 3. Tell EF Core this entity is Modified
            tracker.State = EntityState.Modified;

            // 4. Save the changes to DB
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            //// Turning off Tracking for UpdateAsync in Entity Framework
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();

            /*
            try
            {
                // Get primary key dynamically
                var keyValues = _context.Model.FindEntityType(typeof(T))
                                ?.FindPrimaryKey()
                                ?.Properties
                                ?.Select(p => p.PropertyInfo.GetValue(entity))
                                .ToArray();

                if (keyValues == null || keyValues.Length == 0)
                    throw new InvalidOperationException("No primary key defined for entity.");

                // Fetch existing entity without tracking
                var existingEntity = await _context.Set<T>().FindAsync(keyValues);

                if (existingEntity == null) return 0;

                _context.Entry(existingEntity).State = EntityState.Detached; // ✅ Prevent tracking conflicts
                _context.Entry(entity).State = EntityState.Modified; // ✅ Mark for update

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }           
             */
        }

        // ==========================
        // === Remove
        // ==========================

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        #region Separating asigned entity and save operators        

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators
    }

}
