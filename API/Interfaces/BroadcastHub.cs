using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR; 

namespace API.Interfaces
{
    public class BroadcastHub: Hub<IHubClient>  
    {
       public async Task Task2Hub()
       {
            await Clients.Others.Task2();
       }
    }
}