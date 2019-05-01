using archive.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace archive.Commons.Authorization
{
    public class SolutionAuthorizationHandler :
        AuthorizationHandler<ModOrOwnerRequirement, Data.Entities.Solution>
    {
        private readonly UserManager<Data.Entities.ApplicationUser> _userManager;

        public SolutionAuthorizationHandler(UserManager<Data.Entities.ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ModOrOwnerRequirement requirement, Data.Entities.Solution resource)
        {
            if (_userManager.GetUserId(context.User) == resource.AuthorId)
            {
                context.Succeed(requirement);
            }

            var user = await _userManager.GetUserAsync(context.User);
            if (await _userManager.IsInRoleAsync(user, UserRoles.MODERATOR))
            {
                context.Succeed(requirement);
            }
        }
    }

    public class ModOrOwnerRequirement : IAuthorizationRequirement { }
}
