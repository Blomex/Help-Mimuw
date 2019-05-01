using System.Threading.Tasks;
using archive.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace archive.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly IUserActivityService _activityService;

        public ArchiveController(IUserActivityService activityService)
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