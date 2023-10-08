using DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface ICommentsService
    {
        Task<List<Comment>> GetComments(long callId,CancellationToken cancellationToken);
        Task<bool> SubmitComment(long callId,string comment,CancellationToken cancellationToken);
    }
}
