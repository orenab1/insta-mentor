using System.Collections.Generic;
using DAL.Entities;

namespace DAL.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}