using Domain;
using Microsoft.EntityFrameworkCore;

namespace CrudToDo.Persistence
{
    public class CoreDbContext : DbContext
    {
        public DbSet<ActionItem> ActionItems { get; set; }
        public DbSet<User> Users { get; set; }

        public CoreDbContext() { }

        public CoreDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // No configuration for the EF or context services
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // No configuration for the entities in the database
        }
    }
}
