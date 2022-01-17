using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTOs;

namespace DAL.Interfaces
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<CommunityDto>> GetCommunities();

        Task<bool> UpdateCommunitiesForUser(CommunityDto[] newCommunities, int userId);
    }
}