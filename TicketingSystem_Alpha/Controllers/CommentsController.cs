using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem_Alpha.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(string comment,long callId,CancellationToken cancellationToken)
        {
            var resp = await _commentsService.SubmitComment(callId, comment, cancellationToken);
            if(resp)
                return Ok();
            else
                return BadRequest("An Error Has Occured");
        }
    }
}
