using Microsoft.EntityFrameworkCore;

namespace TrainTracker.Api.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



}
