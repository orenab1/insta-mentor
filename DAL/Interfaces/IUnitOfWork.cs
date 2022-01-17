using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IQuestionRepository QuestionRepository { get; }

        ITagRepository TagRepository { get; }

        ICommunityRepository CommunityRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}