using TrainTracker.Api.Models.DTOs;

namespace TrainTracker.Api.Services.Interfaces;

public interface IStationsService
{
  Task<List<StationDto>> SearchStations(string query);
}
