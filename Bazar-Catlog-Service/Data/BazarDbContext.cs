using Bazar_Catlog_Service.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazar_Catlog_Service.Data;

public class BazarDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Bazar.db");
        }
    }

    public BazarDbContext(DbContextOptions options) : base(options)
    {
        
    }
}