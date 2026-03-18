using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class EmpContext: DbContext
    {
        public EmpContext(DbContextOptions<EmpContext> options) : base(options)
        {
        }
            public DbSet<Department> departments { get; set; }
            public DbSet<Employee> employees { get; set; }
    }
}
