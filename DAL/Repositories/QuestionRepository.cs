using System.Configuration;
using DAL.Interfaces;
using DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DAL.DTOs;
using System.Linq;

namespace DAL.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public QuestionRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            return await _mapper
                .ProjectTo<QuestionDto>(_context.Questions.Where(x => x.Id == id))
                .SingleOrDefaultAsync();
        }

        public async void AskQuestion(QuestionDto questionDto)
        {
            _context.Questions.AddAsync(new Question
            {
                Header = questionDto.Header,
                Body = questionDto.Body
            });
            
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}