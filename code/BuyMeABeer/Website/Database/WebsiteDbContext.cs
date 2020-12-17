using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Website.Database
{
    public class WebsiteDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public WebsiteDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                .HasOne(comment => comment.Payment)
                .WithOne(payment => payment.Comment);
        }
    }
}
