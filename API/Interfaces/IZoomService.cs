using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IZoomService
    {
        Task<string> GetMeetingUrl(string topic);
    }
}