using Microsoft.EntityFrameworkCore;
using MYPM.Data.Models;

namespace MYPM.Data.Configurations;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NewOrderModel> Orders { get; set; }
    public DbSet<PanjabiOrder> PanjabiOrders { get; set; }
    public DbSet<ArabianOrder> ArabianOrders { get; set; }
    public DbSet<SelowerOrder> SelowerOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NewOrderModel>()
            .HasMany(o => o.PanjabiOrders)
            .WithOne(p => p.NewOrder)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewOrderModel>()
            .HasMany(o => o.ArabianOrders)
            .WithOne(a => a.NewOrder)
            .HasForeignKey(a => a.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NewOrderModel>()
            .HasMany(o => o.SelowerOrders)
            .WithOne(s => s.NewOrder)
            .HasForeignKey(s => s.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}