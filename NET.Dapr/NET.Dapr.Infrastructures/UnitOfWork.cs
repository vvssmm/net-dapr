using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Infra;

namespace NET.Dapr.Infrastructures
{
    public class UnitOfWork(PgDbContext pgDbContext) : IUnitOfWork
    {
        readonly PgDbContext _pgDbContext = pgDbContext;
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _pgDbContext.Set<T>();
        }

        public int SaveChanges()
        {
            return _pgDbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _pgDbContext.SaveChangesAsync();
        }
    }
}
