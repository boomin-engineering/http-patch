using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpPatch.Tests.Api
{
    public class SystemTextStartup
    {
        public SystemTextStartup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        [SuppressMessage("", "CA1822", Justification = "Called by runtime")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        [SuppressMessage("", "CA1822", Justification = "Called by runtime")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}