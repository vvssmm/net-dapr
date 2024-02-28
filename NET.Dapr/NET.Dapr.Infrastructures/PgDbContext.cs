using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NET.Dapr.Domains.Entities;

namespace NET.Dapr.Infrastructures
{
    public class PgDbContext(IConfiguration configuration) : DbContext
    {
        protected readonly IConfiguration Configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityByDefaultColumns();
            modelBuilder.Entity<ApproverConfig>()
                .Property(b => b.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<EmailHistories>()
                .Property(b => b.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Task>()
                .Property(b => b.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<LRTransaction>()
                .Property(b => b.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<WorkflowFormConfig>()
              .Property(b => b.Id).HasIdentityOptions(startValue: 1000);

            base.OnModelCreating(modelBuilder);
        }
        private void AutoUpdateDate()
        {
            var entryList = ChangeTracker.Entries();
            foreach (var entry in entryList)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedDate = DateTime.UtcNow;
                    }
                }
            }
        }
        public override int SaveChanges()
        {
            AutoUpdateDate();
            return base.SaveChanges();
        }
        public DbSet<ApproverConfig> ApproverConfig { get; set; }
        public DbSet<EmailHistories> EmailHistories { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<LRTransaction> LRTransactions { get; set; }
        public DbSet<WorkflowFormConfig> WorkflowFormConfig { get; set; }
    }
}
