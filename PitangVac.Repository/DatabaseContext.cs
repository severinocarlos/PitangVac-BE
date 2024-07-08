using Microsoft.EntityFrameworkCore;
using PitangVac.Entity.Entities;

namespace PitangVac.Repository
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Scheduling> Scheduling { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
