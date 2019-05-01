using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace archive.Tests.Integration
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class _IgnoreAntiforgeryTokenAttribute : Attribute, IAntiforgeryPolicy, IFilterMetadata, IOrderedFilter
    {
        public int Order { get; set; } = 1337;
    }

    public class WebApplicationFactoryWithoutAuthentication : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services
                    .AddMvc(o =>
                    {
                        o.Filters.Add(new AllowAnonymousFilter());
                        o.Filters.Add(new _IgnoreAntiforgeryTokenAttribute());
                    });
            });
        }
    }
}