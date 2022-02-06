using System.Threading.Tasks; 

namespace API.Interfaces
{
    public interface IHubClient
    {

        Task BroadcastMessage();  
        Task Task2();
    }
}