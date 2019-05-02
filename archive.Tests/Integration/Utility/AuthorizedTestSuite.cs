using archive.Data;
using archive.Data.Entities;
using archive.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace archive.Tests.Integration.Utility
{
    class AuthorizedTestSuite
    {
        protected WebApplicationFactory<Startup> factory;

        /// <summary>
        /// User without any roles (too lazy).
        /// </summary>
        protected ApplicationUser lenesia;

        /// <summary>
        /// User with TRUSTED_USER role
        /// </summary>
        protected ApplicationUser marielle;

        /// <summary>
        /// Another user with TRUSTED_USER role
        /// </summary>
        protected ApplicationUser henrietta;

        /// <summary>
        /// User with TRUSTED_USER and MODERATOR roles
        /// </summary>
        protected ApplicationUser akatsuki;

        /// <summary>
        /// User with TRUSTED_USER, MODERATOR and IMPERATOR roles
        /// </summary>
        protected ApplicationUser shiroe;


        public AuthorizedTestSuite()
        {
            factory = new WebApplicationFactory<Startup>();
            /* Need to create client before doing anything */
            factory.CreateDefaultClient();

            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                lenesia = GetUser(scope, "Lenesia", new string[] {});
                marielle = GetUser(scope, "Marielle", new string[] { UserRoles.TRUSTED_USER });
                henrietta = GetUser(scope, "Henrietta", new string[] { UserRoles.TRUSTED_USER });
                akatsuki = GetUser(scope, "Akatsuki", new string[] { UserRoles.TRUSTED_USER, UserRoles.MODERATOR });
                shiroe = GetUser(scope, "Shiroe", new string[] { UserRoles.TRUSTED_USER, UserRoles.MODERATOR, UserRoles.IMPERATOR });
            }
        }

        protected TController LoginToController<TController>(IServiceScope scope, ApplicationUser asUser)
            where TController : Controller
        {
            var controller = scope.ServiceProvider.GetRequiredService<TController>();
            var claimFactory = scope.ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var claimJob = claimFactory.CreateAsync(asUser);
            claimJob.Wait();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimJob.Result }
            };
            return controller;
        }

        protected Data.Entities.ApplicationUser
            GetUser(IServiceScope scope, string name, IEnumerable<string> roles)
        {
            var t = GetUserAsync(scope, name, roles);
            t.Wait();
            return t.Result;
        }

        private async Task<ApplicationUser>
            GetUserAsync(IServiceScope scope, string name, IEnumerable<string> roles)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(name);

            if (user == null)
            {
                user = new ApplicationUser { UserName = name };
                await userManager.CreateAsync(user);
                await userManager.AddToRolesAsync(user, roles);

                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await db.SaveChangesAsync();
            }

            return user;
        }
    }
}
