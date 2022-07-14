using Microsoft.EntityFrameworkCore;
using Samp.Core.Extensions;
using Samp.Core.Model;
using Samp.Movie.Database;
using Samp.Movie.Database.Migrations;

namespace Samp.Movie.API
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
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGlobalStartupServices<MovieApplicationSettings>(Configuration);

            var IdentityContext = new DbContextParameter<MovieDbContext, MovieDbContextSeed>((provider, opt) =>
                    opt.UseInMemoryDatabase(databaseName: nameof(MovieDbContext)).EnableSensitiveDataLogging());

            services.AddCustomDbContext(IdentityContext);
        }
    }
}