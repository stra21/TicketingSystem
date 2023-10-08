using DL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Utilities
{
    public class ChangesIntercepor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _accessor;
        public ChangesIntercepor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
               if(entry.Entity.GetType()==typeof(Call) || entry.Entity.GetType().BaseType == typeof(Call))
                {
                    var call = (Call)entry.Entity;
                    if(entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        call.ModificationDateTime = DateTime.UtcNow;
                        call.FkModifiedBy = int.Parse(_accessor.HttpContext.User.Claims.Where(x=>x.Type=="ID").First().Value);
                    }
                    else if(entry.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                    {
                        call.DeletionDateTime = DateTime.UtcNow;
                        call.FkDeletedBy = int.Parse(_accessor.HttpContext.User.Claims.Where(x => x.Type == "ID").First().Value);
                        call.IsDeleted = true;
                        entry.Context.Update(call);
                        entry.Context.SaveChangesAsync();
                        var res = InterceptionResult<int>.SuppressWithResult(1);
                        return new ValueTask<InterceptionResult<int>>(res);
                    }
                    else
                    {
                        call.CreationDateTime = DateTime.UtcNow;
                        call.FkCreatedBy = int.Parse(_accessor.HttpContext.User.Claims.Where(x => x.Type == "ID").First().Value);
                    }
                }
                else if(entry.Entity.GetType()==typeof(Comment))
                {
                    var comment = (Comment)entry.Entity;
                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        comment.FkCommenterId = int.Parse(_accessor.HttpContext.User.Claims.Where(x => x.Type == "ID").First().Value);
                    }
                }

            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
