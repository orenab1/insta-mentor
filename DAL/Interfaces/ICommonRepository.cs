using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTOs;

namespace DAL.Interfaces
{
    public interface ICommonRepository
    {
        Task<IEnumerable<TagDto>> GetTagsByCreatorOrApproved(int creatorId);
    }
}