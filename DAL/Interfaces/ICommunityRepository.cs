using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTOs;
using System;

namespace DAL.Interfaces
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<CommunityDto>> GetCommunities();

        List<CommunityFullDto> GetCommunitiesSummaries(int currentUserId);        

        int GetNumOfUsersInCommunity(int communityId);

        Task<bool> DeleteCommunity(int communityId);

        Task<bool> LeaveCommunity(int communityId, int userId);

        Task<bool> JoinCommunity(int communityId, int userId);

        Task<CommunityFullDto> GetCommunity(int communityId,int currentUserId);

        Task<DateTime?> LastCreatedCommunity(int userId);

        Task<bool> CreateCommunity(AddCommunityDto addCommunityDto, int userId);

        Task<bool> IsCommunityNameExists(string communityName);
    }
}