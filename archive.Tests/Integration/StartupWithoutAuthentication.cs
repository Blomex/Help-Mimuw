using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace archive.Tests.Integration
{
    public class StartupWithoutAuthentication : Startup
    {
        public StartupWithoutAuthentication(IConfiguration configuration) : base(configuration)
        {
        }

        public override void AddMvc(IServiceCollection services)
        {
            services.AddMvc(opts => { opts.Filters.Add(new AllowAnonymousFilter()); })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}