using Microsoft.EntityFrameworkCore;
using TrainTracker.Api.Data;
using TrainTracker.Api.Hubs;
using TrainTracker.Api.Services.Implementations;
using TrainTracker.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      policy => policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
//HttpClient
builder.Services.AddHttpClient<ITrainApiService, TrainApiService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapHub<TrainHub>("/trainHub");
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();

app.Run();
