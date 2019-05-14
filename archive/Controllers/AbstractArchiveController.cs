using System.Threading.Tasks;
using archive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace archive.Controllers
{
    public abstract class AbstractArchiveController : Controller
    {
        private readonly IUserActivityService _activityService;

        protected AbstractArchiveController(IUserActivityService activityService)
        {
            _activityService = activityService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            base.OnActionExecuting(context);
            if (User?.Identity?.Name != null)
            {
                await _activityService.RegisterActionAsync(User.Identity.Name);
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}