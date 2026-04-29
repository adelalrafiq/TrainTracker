using Microsoft.EntityFrameworkCore;
using TrainTracker.Api.Models;

namespace TrainTracker.Api.Data;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options) { }
  public DbSet<Station> Stations { get; set; }
  public DbSet<Train> Trains { get; set; }
  public DbSet<Trip> Trips { get; set; }
  public DbSet<DelayRecord> DelayRecords { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Trip>()
    .HasOne(t => t.Train)
    .WithMany(t => t.Trips)
    .HasForeignKey(t => t.TrainId)
    .OnDelete(DeleteBehavior.Restrict);
    modelBuilder.Entity<Trip>()
        .HasOne(t => t.DepartureStation)
        .WithMany(s => s.DepartureTrips)
        .HasForeignKey(t => t.DepartureStationId)
        .OnDelete(DeleteBehavior.Restrict);
    modelBuilder.Entity<Trip>()
        .HasOne(t => t.ArrivalStation)
        .WithMany(s => s.ArrivalTrips)
        .HasForeignKey(t => t.ArrivalStationId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
