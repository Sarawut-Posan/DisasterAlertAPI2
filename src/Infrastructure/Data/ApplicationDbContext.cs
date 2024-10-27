using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Region> Regions { get; set; }
    public DbSet<AlertSetting> AlertSettings { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<DisasterRisk> DisasterRisks { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>()
            .HasIndex(r => r.RegionID)
            .IsUnique();
        modelBuilder.Entity<AlertSetting>()
            .HasKey(a => new { a.RegionId, a.DisasterType });
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Timestamp).HasDefaultValueSql("NOW()");
        });
        modelBuilder.Entity<DisasterRisk>()
            .HasKey(dr => new { dr.RegionId, dr.DisasterType });
    }
}