using JWT_AUTHENTICATION.Models;
using JWT_AUTHENTICATION.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace JWT_AUTHENTICATION.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        //public DbSet<RegisterRequest> Requests {get; set;}
        //public DbSet<LoginRequest> LoginRequests {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
