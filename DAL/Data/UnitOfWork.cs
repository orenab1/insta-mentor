using System.Threading.Tasks;
using DAL.Interfaces;
using AutoMapper;
using DAL.Repositories;

namespace DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IQuestionRepository QuestionRepository => new QuestionRepository(_context, _mapper,TagRepository);
        public ITagRepository TagRepository => new TagRepository(_context, _mapper);
        public ICommonRepository CommonRepository => new CommonRepository(_context, _mapper);

        public IAccountRepository AccountRepository => new AccountRepository(_context, _mapper);

        public ICommunityRepository CommunityRepository => new CommunityRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}