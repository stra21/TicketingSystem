using BL.Enums;
using BL.Interfaces;
using DL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace TicketingSystem_Alpha.Controllers
{
    [Authorize]
    public class CallsController : Controller
    {
        private readonly ICallsService _callsService;
        private readonly IModulesService _modulesService;
        private readonly ICallTypesService _callTypesService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUploadsService _uploadsService;
        private readonly ICommentsService _commentsService;
        private readonly IUsersService _usersService;
        public CallsController(ICallsService callsService, IModulesService modulesService, ICallTypesService callTypesService,
            IHttpContextAccessor contextAccessor, IUploadsService uploadsService,ICommentsService commentsService,IUsersService usersService)
        {
            _callsService = callsService;
            _modulesService = modulesService;
            _callTypesService = callTypesService;
            _contextAccessor = contextAccessor;
            _uploadsService = uploadsService;
            _commentsService = commentsService;
            _usersService = usersService;

        }
        public async Task<IActionResult> Calls(CancellationToken cancellationToken = default)
        {
            var modules = await _modulesService.Get(cancellationToken);
            var callTypes = await _callTypesService.Get(cancellationToken);
            var calls = await _callsService.GetCalls(cancellationToken);
            ViewBag.modules = modules;
            ViewBag.callTypes = callTypes;
            return View(calls);
        }
        public async Task<IActionResult> Call(long? Id, CancellationToken cancellationToken = default)
        {
            var modules = await _modulesService.Get(cancellationToken);
            var callTypes = await _callTypesService.Get(cancellationToken);
            var calls = await _callsService.GetCall(x => x.Id == Id, cancellationToken);
            string userType = _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Type").Value;
            string userId = _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "ID").Value;
            var users = await _usersService.GetUsers(x => x.UserType == (int)SystemRolesEnum.ERPTeamMember, cancellationToken);
            List<SelectListItem> priorities = new List<SelectListItem>()
            {
                new SelectListItem(Priority.ShowStopper.ToString(),((int)Priority.ShowStopper).ToString(),calls.Priority ==(int)Priority.ShowStopper),
                new SelectListItem(Priority.High.ToString(),((int)Priority.High).ToString(),calls.Priority ==(int)Priority.High),
                new SelectListItem(Priority.Medium.ToString(),((int)Priority.Medium).ToString(), calls.Priority ==(int) Priority.Medium),
                new SelectListItem(Priority.Low.ToString(),((int)Priority.Low).ToString(), calls.Priority ==(int) Priority.Low),
            };
            List<SelectListItem> supportStatus = new List<SelectListItem>()
            {
                new SelectListItem(SupportStatuses.Pending.ToString(),((int)SupportStatuses.Pending).ToString(),calls.SupportStatus ==(int)SupportStatuses.Pending),
                new SelectListItem(SupportStatuses.WIP.ToString(),((int)SupportStatuses.WIP).ToString(),calls.SupportStatus ==(int)SupportStatuses.WIP),
                new SelectListItem(SupportStatuses.Delivered.ToString(),((int)SupportStatuses.Delivered).ToString(), calls.SupportStatus ==(int) SupportStatuses.Delivered),
            };
            List<SelectListItem> userStatus = new List<SelectListItem>()
            {
                new SelectListItem(UserStatuses.Pending.ToString(),((int)UserStatuses.Pending).ToString(),calls.UserStatus ==(int)UserStatuses.Pending),
                new SelectListItem(UserStatuses.Reopened.ToString(),((int)UserStatuses.Reopened).ToString(),calls.UserStatus ==(int)UserStatuses.Reopened),
                new SelectListItem(UserStatuses.Closed.ToString(),((int)UserStatuses.Closed).ToString(), calls.UserStatus ==(int) UserStatuses.Closed),
            };
            List<SelectListItem> refCalls = new List<SelectListItem>();
            refCalls.AddRange((await _callsService.GetCalls(cancellationToken)).Where(x=>x.Id !=Id).Select(x => new SelectListItem(x.Subject, x.Id.ToString(), calls.FkRefCall == x.FkRefCall)));
            ViewBag.refCalls = refCalls;
            ViewBag.modules = modules;
            ViewBag.callTypes = callTypes;
            ViewBag.priorities = priorities;
            ViewBag.supportStatus = supportStatus;
            ViewBag.userStatus = userStatus;
            ViewBag.userType = int.Parse(userType);
            ViewBag.userId = int.Parse(userId);
            ViewBag.users = users;
            return View(calls);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Call call, List<IFormFile>? postedFile, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                var resp = await _callsService.UpdateCall(call, cancellationToken);
                if(postedFile is not null && postedFile.Count>0)
                    _=await _uploadsService.UploadFiles(postedFile, call.Id,cancellationToken);
                return RedirectToAction("Call", new { Id = resp.Id });
            }
            var modules = await _modulesService.Get(cancellationToken);
            var callTypes = await _callTypesService.Get(cancellationToken);
            var calls = await _callsService.GetCall(x => x.Id == call.Id, cancellationToken);
            string userType = _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Type").Value;
            string userId = _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "ID").Value;
            var users = await _usersService.GetUsers(x => x.UserType == (int)SystemRolesEnum.ERPTeamMember, cancellationToken);
            List<SelectListItem> priorities = new List<SelectListItem>()
            {
                new SelectListItem(Priority.ShowStopper.ToString(),((int)Priority.ShowStopper).ToString(),calls.Priority ==(int)Priority.ShowStopper),
                new SelectListItem(Priority.High.ToString(),((int)Priority.High).ToString(),calls.Priority ==(int)Priority.High),
                new SelectListItem(Priority.Medium.ToString(),((int)Priority.Medium).ToString(), calls.Priority ==(int) Priority.Medium),
                new SelectListItem(Priority.Low.ToString(),((int)Priority.Low).ToString(), calls.Priority ==(int) Priority.Low),
            };
            List<SelectListItem> supportStatus = new List<SelectListItem>()
            {
                new SelectListItem(SupportStatuses.Pending.ToString(),((int)SupportStatuses.Pending).ToString(),calls.SupportStatus ==(int)SupportStatuses.Pending),
                new SelectListItem(SupportStatuses.WIP.ToString(),((int)SupportStatuses.WIP).ToString(),calls.SupportStatus ==(int)SupportStatuses.WIP),
                new SelectListItem(SupportStatuses.Delivered.ToString(),((int)SupportStatuses.Delivered).ToString(), calls.SupportStatus ==(int) SupportStatuses.Delivered),
            };
            List<SelectListItem> userStatus = new List<SelectListItem>()
            {
                new SelectListItem(UserStatuses.Pending.ToString(),((int)UserStatuses.Pending).ToString(),calls.UserStatus ==(int)UserStatuses.Pending),
                new SelectListItem(UserStatuses.Reopened.ToString(),((int)UserStatuses.Reopened).ToString(),calls.UserStatus ==(int)UserStatuses.Reopened),
                new SelectListItem(UserStatuses.Closed.ToString(),((int)UserStatuses.Closed).ToString(), calls.UserStatus ==(int) UserStatuses.Closed),
            };
            List<SelectListItem> refCalls = new List<SelectListItem>();
            refCalls.AddRange((await _callsService.GetCalls(cancellationToken)).Where(x => x.Id != call.Id).Select(x => new SelectListItem(x.Subject, x.Id.ToString(), calls.FkRefCall == x.FkRefCall)));
            ViewBag.refCalls = refCalls;
            ViewBag.modules = modules;
            ViewBag.callTypes = callTypes;
            ViewBag.priorities = priorities;
            ViewBag.supportStatus = supportStatus;
            ViewBag.userStatus = userStatus;
            ViewBag.userType = int.Parse(userType);
            ViewBag.userId = int.Parse(userId);
            ViewBag.users = users;
            return View("Call", call);
        }
        [HttpGet("Calls/Delete/{callId}")]
        public async Task<IActionResult> Delete(long callId, CancellationToken cancellationToken = default)
        {
            var resp = await _callsService.DeleteCall(callId, cancellationToken);
            return RedirectToAction("Calls");
        }
    }
}
