using Microsoft.OpenApi;
using TrainTracker.Api.Services.Implementations;
using TrainTracker.Api.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddHttpClient<ILiveboardService, LiveboardService>();
builder.Services.AddScoped<IStationsService, StationsService>();

// Swagger/OpenAPI configuration
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "🚆 TrainTracker API",
    Version = "v1",
    Description = "Real-time train departure board API using iRail data.",
    Contact = new OpenApiContact
    {
      Name = "Adel Al-Rafiq",
      Email = "adelalrafiq@gmail.com"
    }
  });
});

//builder.Services.AddCors();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularClient", policy =>
  {
    policy.WithOrigins(allowedOrigins ?? [])
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
  });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
  c.InjectStylesheet("/swagger-ui/custom.css");
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainTracker API v1");
  c.DocumentTitle = "TrainTracker API Docs";
  c.RoutePrefix = "swagger";
});

app.UseCors("AllowAngularClient");
app.UseAuthorization();

app.MapControllers();

app.Run();
