using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Data
{
    public class SqliteDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public SqliteDbContext(IConfiguration Configuration, DbContextOptions<SqliteDbContext> options) : base(options)
        {
            _configuration = Configuration;
        }

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             modelBuilder.Entity<User>();
         }*/
        public DbSet<UserRegister> Users { get; set; } = null!;
    }

}
