https://www.postman.com/downloads/
https://www.microsoft.com/en-us/download/details.aspx?id=58494

-- Libraries parse vào repositories

dotnet add package Microsoft.EntityFrameworkCore --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.5
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.5
dotnet add package Microsoft.Extensions.Configuration --version 8.0.0
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.0

------------

public static string GetConnectionString(string connectionStringName)
{
    var config = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    string connectionString = config.GetConnectionString(connectionStringName);
    return connectionString;
}

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
-- Create folder
    
DBContext
GenericRepository

-- Add GenericRepository

public class GenericRepository<T> where T : class
{
    protected zPaymentContext _context;

    public GenericRepository()
    {
        _context ??= new zPaymentContext();
    }

    public GenericRepository(zPaymentContext context)
    {
        _context = context;
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public void Create(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public async Task<int> CreateAsync(T entity)
    {
        _context.Add(entity);
        return await _context.SaveChangesAsync();
    }
    public void Update(T entity)
    {
        //// Turning off Tracking for UpdateAsync in Entity Framework
        _context.ChangeTracker.Clear();
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
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

    public bool Remove(T entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> RemoveAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public async Task<T> GetByIdAsync(int id)
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

-- JSON Web Token

dotnet add package System.IdentityModel.Tokens.Jwt --version 8.3.0

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    });






