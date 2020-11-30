using Microsoft.EntityFrameworkCore;

namespace Website.Database
{
    public class WebsiteDbContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public WebsiteDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
