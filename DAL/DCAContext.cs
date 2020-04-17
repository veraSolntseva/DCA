using DAL.DbObjects;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DCAContext : DbContext
    {
        public DbSet<FirstSheetItem> FirstSheetItems { get; set; }

        public DbSet<SecondSheetItem> SecondSheetItems { get; set; }


        public DCAContext(DbContextOptions<DCAContext> options) : base(options) { }
    }
}
