using TrainTracker.Api.Models.DTOs;

namespace TrainTracker.Api.Services.Interfaces;

public interface IConnectionsService
{
  Task<List<ConnectionDto>> GetConnectionsAsync(
    string from,
    string to,
    string? date = null,
    string? time = null);
}
