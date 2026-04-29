using Microsoft.AspNetCore.SignalR;
namespace TrainTracker.Api.Hubs;

public class TrainHub : Hub
{
  public async Task SendMessage(string message)
  {
    await Clients.All.SendAsync("ReceiveMessage", message);
  }
}
