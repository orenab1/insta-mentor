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
using System;

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

        public async Task<int> AskQuestion(QuestionDto questionDto)
        {
            var questionToSave=new Question
            {
                Header = questionDto.Header,
                Body = questionDto.Body
            };

            _context.Questions.AddAsync(questionToSave);
            
            await _context.SaveChangesAsync();


            return questionToSave.Id;

        }
    }
}