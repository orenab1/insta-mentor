using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTOs;

namespace DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagDto>> GetTagsByCreatorOrApproved(int creatorId);

        Task<bool> UpdateTagsForUser(TagDto[] newTags, int userId);

        void AddTagsToDBAndAssignId(ref TagDto[] tags, int creatorUserId);
    }
}