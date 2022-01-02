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
//using System.Runtime.Remoting.Lifetime;

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

        public async Task<int> AskQuestionAsync(QuestionDto questionDto)
        {
            var questionToSave = new Question
            {
                Header = questionDto.Header,
                Body = questionDto.Body
            };

            _context.Questions.AddAsync(questionToSave);

            await _context.SaveChangesAsync();


            return questionToSave.Id;

        }

        public async Task<bool> PostCommentAsync(CommentDto commentDto)
        {
            var question = _context.Questions.SingleOrDefault(x => x.Id == commentDto.QuestionId);

            if (question.Comments==null)
            {
                question.Comments=new List<Comment>();
            }
            question.Comments.Add(
                new Comment
                {
                    Text = commentDto.Text,
                    Created = DateTime.Now,
                    Question = question
                }
            );


            _context.Entry(question).State = EntityState.Modified;

            return await _context.SaveChangesAsync()>0;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync()
        {
            return await _mapper
                .ProjectTo<QuestionDto>(_context.Questions)
                .ToListAsync();
        }
    }
}