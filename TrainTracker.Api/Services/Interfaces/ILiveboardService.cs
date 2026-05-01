using TrainTracker.Api.Models.DTOs;

namespace TrainTracker.Api.Services.Interfaces;

public interface ILiveboardService
{
  Task<LiveboardDto> GetLiveboard(string station);
}
