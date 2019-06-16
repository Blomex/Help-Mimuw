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
using archive.Services.Users;
using Markdig;
using Markdig.Extensions.Mathematics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Task = System.Threading.Tasks.Task;

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
            services.AddScoped<IAchievementsService, AchievementsService>();
            services.AddScoped(typeof(MarkdownPipeline), s =>
            {
                var pipeline = new MarkdownPipelineBuilder();
                pipeline.Use<MathExtension>();
                return pipeline.Build();
            });
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

            Task rolesCreationTask = CreateUserRoles(serviceProvider);
            Task achievementCreationTask = CreateAchievements(serviceProvider);
            rolesCreationTask.Wait();
            achievementCreationTask.Wait();
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

        protected async Task CreateUserRoles(IServiceProvider serviceProvider)
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

        protected async Task CreateAchievements(IServiceProvider serviceProvider)
        {
            var achievements = serviceProvider.GetRequiredService<IAchievementsService>();
            //TODO : ustalenie jakiejś arbitralnej wielkości ikon i dodanie ikon do achievementów
            var redaktor1 = new Data.Entities.Achievement
            {
                Id = 1,
                Name = "Redaktor I",
                NormalizedName = "REDAKTOR I",
                Description = "Uzyskany za dodanie pierwszego rozwiązania",
                AchievementFlags = AchievementFlags.None,
                IconPath = "redaktor1.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var redaktor2 = new Data.Entities.Achievement
            {
                Id = 2,
                Name = "Redaktor II",
                NormalizedName = "REDAKTOR II",
                Description = "Uzyskany za dodanie 3 rozwiązań",
                AchievementFlags = AchievementFlags.None,
                IconPath = "redaktor2.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var redaktor3 = new Data.Entities.Achievement
            {
                Id = 3,
                Name = "Redaktor III",
                NormalizedName = "REDAKTOR III",
                Description = "Uzyskany za dodanie 6 rozwiązań",
                AchievementFlags = AchievementFlags.None,
                IconPath = "redaktor3.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var redaktor4 = new Data.Entities.Achievement
            {
                Id = 4,
                Name = "Redaktor IV",
                NormalizedName = "REDAKTOR IV",
                Description = "Uzyskany za dodanie 10 rozwiązań",
                AchievementFlags = AchievementFlags.None,
                IconPath = "redaktor4.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var redaktor5 = new Data.Entities.Achievement
            {
                Id = 5,
                Name = "Redaktor V",
                NormalizedName = "REDAKTOR V",
                Description = "Uzyskany za dodanie 20 rozwiązań",
                AchievementFlags = AchievementFlags.None,
                IconPath = "redaktor5.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            
            var GoracyTemat = new Data.Entities.Achievement
            {
                Id = 6,
                Name = "Gorący Temat",
                NormalizedName = "GORĄCY TEMAT",
                Description = "Uzyskany, gdy pod jednym z twoich rozwiązań pojawiło się 42 komentarzy",
                AchievementFlags = AchievementFlags.None,
                IconPath = "goracyTemat.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };

            var SokoleOko = new Data.Entities.Achievement
            {
                Id = 7,
                Name = "Sokole Oko",
                NormalizedName = "SOKOLE OKO",
                Description = "Uzyskany, za użycie sformułowania 'widać, że' w rozwiązaniu",
                AchievementFlags = AchievementFlags.None,
                IconPath = "sokoleOko.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var IdealneRozwiazanie = new Data.Entities.Achievement
            {
                Id = 8,
                Name = "Idealne Rozwiązanie",
                NormalizedName = "IDEALNE ROZWIĄZANIE",
                Description = "Jedno z twoich rozwiązań otrzymało 10 głosów i wszystkie były pozytywne!",
                AchievementFlags = AchievementFlags.None,
                IconPath = "idealneRozwiazanie.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var PierwszeKroki = new Data.Entities.Achievement
            {
                Id = 6,
                Name = "Pierwsze Kroki",
                NormalizedName = "PIERWSZE KROKI",
                Description = "Uzyskany za dodanie przynajmniej jednego komentarza," +
                              " dodanie przynajmniej jednego rozwiązania, oraz ocenienie przynajmniej 3 rozwiązań",
                AchievementFlags = AchievementFlags.None,
                IconPath = "pierwszeKroki.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            var SzczescieWnieszczesciu = new Data.Entities.Achievement
            {
                Id = 6,
                Name = "Szczęście w nieszczęściu",
                NormalizedName = "SZCZĘŚCIE W NIESZCZĘŚCIU",
                Description = "Przy dodawaniu pliku udało ci się wygenerować konflikt GUID. Gratulacje, nie zdarza się to zbyt często",
                AchievementFlags = AchievementFlags.None,
                IconPath = "szczescieWnieszczesciu.ico",
                UsersAchievements = new HashSet<UsersAchievements>()
            };
            bool x = await achievements.DeclareAchievement(redaktor1);
            await achievements.DeclareAchievement(redaktor2);
            await achievements.DeclareAchievement(redaktor3);
            await achievements.DeclareAchievement(redaktor4);
            await achievements.DeclareAchievement(redaktor5);
            await achievements.DeclareAchievement(GoracyTemat);
            await achievements.DeclareAchievement(SokoleOko);
            await achievements.DeclareAchievement(IdealneRozwiazanie);
            await achievements.DeclareAchievement(PierwszeKroki);
            await achievements.DeclareAchievement(SzczescieWnieszczesciu);
            // Tu deklarujemy achievementy
        }
    }
}
