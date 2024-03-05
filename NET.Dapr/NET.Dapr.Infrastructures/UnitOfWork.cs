using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
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
        private void AutoUpdateDate()
        {
            var entryList = _pgDbContext.ChangeTracker.Entries();
            foreach (var entry in entryList)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.UtcNow;
                        entity.UpdatedDate = DateTime.UtcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedDate = DateTime.UtcNow;
                    }
                }
            }
        }
        public int SaveChanges()
        {
            AutoUpdateDate();
            return _pgDbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            AutoUpdateDate();
            return _pgDbContext.SaveChangesAsync();
        }
    }
}
