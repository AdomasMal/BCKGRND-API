using Microsoft.EntityFrameworkCore;

namespace BCKGRND.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}
