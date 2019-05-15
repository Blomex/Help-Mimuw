using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archive.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using archive.Data;
using archive.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using archive.Data.Entities;
using Microsoft.Extensions.Logging;
using archive.Data.Enums;
using archive.Commons.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;

namespace archive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            
            // Ewentualnie możnaby wymagać potwierdzenia maila przed zalogowaniem się na stronce.
            //Trzeba wtedy dodać odpowiednią opcję w argumencie do AddDefaultIdentity
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EditPolicy", policy =>
                    policy.AddRequirements(new ModOrOwnerRequirement()));
            });
            services.AddScoped<IAuthorizationHandler, SolutionAuthorizationHandler>();
            services.Configure<IdentityOptions>(ConfigureIdentityOptions);
            

            services.AddScoped<IRepository, ApplicationDbContext>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITasksetService, TasksetService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ISolutionService, SolutionService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddMvc().AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes
                    .MapRoute("Storage", "storage/{id}", 
                        defaults: new { controller = "Storage", action = "Index" })
                    .MapRoute("Solution", "solution/{solutionId}",
                        defaults: new { controller = "Solution", action = "Show" })
                    .MapRoute("Solution", "solution/create/{forTaskId}",
                        defaults: new { controller = "Solution", action = "Create" })
                    .MapRoute("Shortcut", "s/{shcCourse}/{shcTaskset?}", new { controller = "Home", action = "Shortcut" })
                    // TODO Może kiedyś się to zrobi:
                    // .MapRoute("shortcut", "s/{shcCourse}/{shcTaskset}/{shcTask?}", new { controller = "Home", action = "Shortcut" })
                    .MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            System.Threading.Tasks.Task rolesCreationTask = CreateUserRoles(serviceProvider);
            rolesCreationTask.Wait();
        }

        protected void ConfigureIdentityOptions(IdentityOptions options)
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 3;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        }

        protected async System.Threading.Tasks.Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Create missing roles");

            foreach (var roleName in UserRoles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    if ((await roleManager.CreateAsync(new IdentityRole(roleName))).Succeeded)
                    {
                        logger.LogInformation($"Created missing role `{roleName}`");
                    }
                    else
                    {
                        logger.LogError($"Failed to create missing role `{roleName}`");
                    }
                }
            }
        }
    }
}
