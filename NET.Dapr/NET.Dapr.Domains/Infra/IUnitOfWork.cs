using Microsoft.EntityFrameworkCore;

namespace NET.Dapr.Domains.Infra
{
    public interface IUnitOfWork
    {
        DbSet<T> GetDbSet<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
