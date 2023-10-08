using BL.Interfaces;
using DL.Interfaces;
using DL.Models;
using DL.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UploadsService : IUploadsService
    {
        private readonly IBaseRepository<UploadedFile> _baseRepository;
        public UploadsService(IBaseRepository<UploadedFile> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<List<UploadedFile>> Get(CancellationToken cancellationToken)
        => await _baseRepository.GetList(x => true, cancellationToken);

        public async Task<List<UploadedFile>> Get(Expression<Func<UploadedFile, bool>> expression, CancellationToken cancellationToken)
        => await _baseRepository.GetList(expression, cancellationToken);
        public async Task<List<UploadedFile>> UploadFiles(List<IFormFile> files, long callId, CancellationToken cancellationToken)
        {
            var response = new List<UploadedFile>();
            _ = await _baseRepository.Delete(x => x.FkCallId == callId, cancellationToken);
            foreach (var file in files)
            {
                string uniqueStamp = Guid.NewGuid().ToString();
                if (!Directory.Exists(Path.Combine("wwwroot", AppSettings.DefaultPath)))
                {
                    Directory.CreateDirectory(Path.Combine("wwwroot",AppSettings.DefaultPath));
                }
                using (var fileStream = new FileStream(Path.Combine("wwwroot", AppSettings.DefaultPath, uniqueStamp + "_" + file.FileName), FileMode.Create,FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                    var upload = new UploadedFile()
                    {
                        FilePath = Path.Combine(AppSettings.DefaultPath, uniqueStamp + "_" + file.FileName),
                        FkCallId = callId
                    };
                    var uploadedFile = await _baseRepository.Create(upload, cancellationToken);
                    response.Add(uploadedFile);
                }
            }
            return response;
        }
    }
}
