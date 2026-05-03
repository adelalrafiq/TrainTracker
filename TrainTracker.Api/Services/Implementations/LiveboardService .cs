using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TrainTracker.Api.Mappings;
using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class LiveboardService : ILiveboardService
{
  private readonly HttpClient _httpClient;
  private readonly IMemoryCache _cache;

  public LiveboardService(HttpClient httpClient, IMemoryCache cache)
  {
    _httpClient = httpClient;
    _cache = cache;
  }
  public async Task<LiveboardDto> GetLiveboard(string station)
  {
    var cacheKey = $"liveboard:{station.ToLower()}";

    // ✅ 1. حاول تجيب من الكاش
    if (_cache.TryGetValue(cacheKey, out LiveboardDto cachedData))
      return cachedData;

    // ❌ 2. إذا مش موجود → اطلب من API
    var freshData = await FetchFromApi(station);

    // ✅ 3. خزّن في الكاش
    var cacheOptions = new MemoryCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30), // مدة الكاش
      SlidingExpiration = TimeSpan.FromSeconds(10) // يتجدد إذا في استخدام
    };

    _cache.Set(cacheKey, freshData, cacheOptions);

    return freshData;
  }

  private async Task<LiveboardDto> FetchFromApi(string station)
  {
    var url = $"https://api.irail.be/liveboard/?station={station}&arrdep=departure&format=json&lang=nl";

    var response = await _httpClient.GetStringAsync(url);

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var data = JsonSerializer.Deserialize<LiveboardResponse>(response, options);

    if (data == null)
      return new LiveboardDto { Rows = new List<LiveboardRowDto>() };

    return data.ToDto(); // ✅ Extension Method
  }
}