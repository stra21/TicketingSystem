using BL.Enums;
using BL.Interfaces;
using DL.Interfaces;
using DL.Models;
using DL.Utilities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Principal;

namespace BL.Services
{
    public class CallsService : ICallsService
    {
        private readonly IBaseRepository<Call> _base;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUploadsService _uploadsService;
        private readonly ICommentsService _commentsService;
        public CallsService(IBaseRepository<Call> @base, IHttpContextAccessor httpContextAccessor, IUploadsService uploadsService, ICommentsService commentsService)
        {
            _base = @base;
            _httpContextAccessor = httpContextAccessor;
            _uploadsService = uploadsService;
            _commentsService = commentsService;
        }

        public async Task<Call> CreateCall(Call call, CancellationToken cancellationToken)
            => await _base.Create(call, cancellationToken);

        public async Task<bool> DeleteCall(long id, CancellationToken cancellationToken)
        => await _base.Delete(x => x.Id == id, cancellationToken);

        public async Task<Call> GetCall(Expression<Func<Call, bool>> expression, CancellationToken cancellationToken)
        {
            var call = await _base.Get(expression, cancellationToken);
            if (call is null)
            {
                call = new Call();
            }
            var uploadedFiles = await _uploadsService.Get(x => x.FkCallId == call.Id, cancellationToken);
            var comments = await _commentsService.GetComments(call.Id, cancellationToken);
            call.UploadedFiles = uploadedFiles.ToList();
            call.Comments = comments;
            return call;
        }

        public async Task<List<Call>> GetCalls(CancellationToken cancellationToken)
        {
            string userType = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "Type").Value;
            string userId = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "ID").Value;
            bool isSystemUser = int.Parse(userType)!=(int)SystemRolesEnum.Guest;
            var calls = await _base.GetList(x => !x.IsDeleted && (isSystemUser || x.FkCreatedBy == int.Parse(userId)), cancellationToken);
            return calls;
        }

        public async Task<Call> UpdateCall(Call call, CancellationToken cancellationToken)
        {
            var oldCall = await _base.Get(x => x.Id == call.Id, cancellationToken);
            if (oldCall is not null && call.SupportStatus != oldCall.SupportStatus)
            {
                call.StatusDate = DateTime.UtcNow;
            }
            else if(oldCall is not null)  
            {
                call.StatusDate = oldCall.StatusDate;
            }
            if (oldCall is not null && call.UserStatus != oldCall.UserStatus && call.UserStatus == (int)UserStatuses.Closed)
            {
                call.ClosingDate = DateTime.UtcNow;
            }
            else if(oldCall is not null)
            {
                call.ClosingDate= oldCall.ClosingDate;
            }
            if(call.FkRefCall ==0)
            {
                call.FkRefCall = null;
            }
            if(call.FkAssignedTo == 0)
            {
                call.FkAssignedTo = null;
            }
            return await _base.Update(call, cancellationToken);
        }
    }
}
