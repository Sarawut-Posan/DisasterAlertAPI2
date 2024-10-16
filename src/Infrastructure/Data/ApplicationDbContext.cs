using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Region> Regions { get; set; }
    public DbSet<AlertSetting> AlertSettings { get; set; }
    public DbSet<Alert> Alerts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>().HasKey(r => r.Id);
        modelBuilder.Entity<AlertSetting>().HasKey(a => new { a.RegionId, a.DisasterType });
        modelBuilder.Entity<Alert>().HasKey(a => a.Id);
    }
}