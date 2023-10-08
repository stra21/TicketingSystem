using DL.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BL.Interfaces
{
    public interface IUploadsService
    {
        Task<List<UploadedFile>> Get(CancellationToken cancellationToken);
        Task<List<UploadedFile>> Get(Expression<Func<UploadedFile, bool>> expression, CancellationToken cancellationToken);
        Task<List<UploadedFile>> UploadFiles(List<IFormFile> files, long callId, CancellationToken cancellationToken);
    }
}
