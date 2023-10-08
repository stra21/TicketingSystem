using BL.Interfaces;
using DL.Interfaces;
using DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IBaseRepository<Comment> _commentsRepository;
        public CommentsService(IBaseRepository<Comment> commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        public async Task<List<Comment>> GetComments(long callId, CancellationToken cancellationToken)
        => await _commentsRepository.GetList(x=>x.FkCallId==callId, cancellationToken);

        public async Task<bool> SubmitComment(long callId, string comment, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(comment) || callId==0)
            {
                return false;
            }
            var savedComment = new Comment()
            {
                FkCallId = callId,
                Comment1 = comment
            };
            savedComment = await _commentsRepository.Create(savedComment,cancellationToken);
            if(savedComment.Id>0)
            {
                return true;
            }
            return false;
        }
    }
}
