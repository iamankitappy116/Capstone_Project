using Microsoft.EntityFrameworkCore;
using MyModels;

namespace repo
{
    public class RepoContext : DbContext
    {
        public RepoContext(DbContextOptions<RepoContext> options) : base(options)
        {

        }

        public DbSet<Car> cars { get; set; }

        public DbSet<Bike> bikes { get; set; }
    }
}
