using TrainTracker.Api.DTOs;

namespace TrainTracker.Api.Services.Interfaces;

public interface ITrainApiService
{
  Task<List<TripDto>> GetLiveBoardAsync(string station);
}
