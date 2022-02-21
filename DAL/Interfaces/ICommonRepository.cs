using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICommonRepository
    {
        Task<int> AddPhoto(Photo photo);

        Task<string> GetPhotoUrl(int id);
    }
}