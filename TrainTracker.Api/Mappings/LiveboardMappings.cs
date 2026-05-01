using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;

namespace TrainTracker.Api.Mappings;

public static class LiveboardMappings
{
  // Entity → DTO (GET)
  public static LiveboardRowDto ToDto(this Departure d, string station)
  {
    return new LiveboardRowDto
    {
      DirectionName = d.Station,
      DepartureTime = ParseUnix(d.Time),
      Platform = d.Platform,
      VehicleInfoShortname = d.VehicleInfo.ShortName,
      DelayMinutes = ParseDelay(d.Delay),
      Status = MapStatus(d)
    };
  }

  // DTO → Entity (POST)
  public static LiveboardDto ToDto(this LiveboardResponse response)
  {
    return new LiveboardDto
    {
      StationName = response.Station,
      Longitude = ParseDouble(response.StationInfo?.LocationX),
      Latitude = ParseDouble(response.StationInfo?.LocationY),
      Rows = response.Departures?.Departure?
      .Take(10)
      .Select(d => d.ToDto(response.Station))
      .OrderBy(x => x.DepartureTime)
      .ToList() ?? new List<LiveboardRowDto>()
    };
  }

  private static DateTime ParseUnix(string unixTime)
  {
    if (long.TryParse(unixTime, out var unixSeconds))
    {
      return DateTimeOffset.FromUnixTimeSeconds(unixSeconds).DateTime;
    }
    return DateTime.MinValue; // Fallback value
  }

  private static int ParseDelay(string delay)
  {
    if (int.TryParse(delay, out var delayMinutes))
    {
      return delayMinutes;
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

  private static double ParseDouble(string value)
  {
    return double.TryParse(value,
        System.Globalization.NumberStyles.Any,
        System.Globalization.CultureInfo.InvariantCulture,
        out var result)
      ? result
      : 0;
  }
}