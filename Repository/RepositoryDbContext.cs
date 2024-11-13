using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryDbContext:DbContext
    {
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options) { }

        public DbSet<User>Users { get; set; }
        public DbSet<Entities.Task>Tasks { get; set; }
    }
}
