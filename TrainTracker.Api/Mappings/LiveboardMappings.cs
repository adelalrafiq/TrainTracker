using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;

namespace TrainTracker.Api.Mappings;

public static class LiveboardMappings
{
  // Entity → DTO (GET)
  public static LiveboardRowDto ToDto(this Departure d, string station)
  {
    var brussels = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
    return new LiveboardRowDto
    {
      DirectionName = d.Station,
      DepartureTime = TimeZoneInfo.ConvertTime(
        DateTimeOffset.FromUnixTimeSeconds(d.Time), brussels),
      Platform = d.Platform,
      VehicleInfoShortname = d.VehicleInfo.ShortName,
      DelayMinutes = ParseDelay(d.Delay),
      Status = MapStatus(d),
      VehicleId = d.Vehicle
    };
  }

  // DTO → Entity (POST)
  public static LiveboardDto ToDto(this LiveboardResponse response)
  {
    return new LiveboardDto
    {
      StationName = response.Station,
      Longitude = response.StationInfo?.LocationX ?? 0,
      Latitude = response.StationInfo?.LocationY ?? 0,
      Rows = response.Departures?.Departure?
      .Select(d => d.ToDto(response.Station))
      .OrderBy(x => x.DepartureTime)
      .ToList() ?? new List<LiveboardRowDto>()
    };
  }

  private static int ParseDelay(string delay)
  {
    if (int.TryParse(delay, out var delaySeconds))
    {
      return (int)Math.Ceiling(delaySeconds / 60.0);
    }
    return 0; // Fallback value
  }

  private static TrainStatus MapStatus(Departure d)
  {
    if (IsCanceled(d.Canceled))
      return TrainStatus.Canceled;

    var delay = ParseDelay(d.Delay);

    if (delay > 0)
      return TrainStatus.Delayed;

    return TrainStatus.OnTime;
  }

  private static bool IsCanceled(string canceled)
  {
    return canceled == "1" || canceled?.ToLower() == "true";
  }
}