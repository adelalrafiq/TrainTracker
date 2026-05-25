using Microsoft.AspNetCore.SignalR;

namespace TrainTracker.Api.Hubs;

public class LiveboardHub : Hub
{
  public override async Task OnConnectedAsync()
  {
    Console.WriteLine("Client connected to SignalR ");
    Console.WriteLine("================================");

    await base.OnConnectedAsync();
  }
}
