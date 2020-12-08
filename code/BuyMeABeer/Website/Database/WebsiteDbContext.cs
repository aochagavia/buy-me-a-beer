using Microsoft.EntityFrameworkCore;
using Website.Database.Entities;

namespace Website.Database
{
    public class WebsiteDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public WebsiteDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
