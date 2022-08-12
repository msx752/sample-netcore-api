using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Samp.Core.Extensions;

namespace Samp.Gateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider isp)
        {
            app.UseGlobalStartupConfigures(env);
            app.UseOcelot().GetAwaiter().GetResult();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGlobalStartupServices<GatewayApplicationSettings>(Configuration);
            services.AddOcelot();
        }
    }
}