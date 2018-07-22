using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalaryPeeker.API.Persistence;
using SalaryPeeker.API.Persistence.Repository;

namespace SalaryPeeker.API
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
            // Add SQLite
            services.AddDbContext<SalaryPeekerDbContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // Configure cross-origin requests
            services.AddCors(options => options.AddPolicy("SalaryPeeperPolicy", policyBuilder =>
            {
                policyBuilder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            }));
            
            // Add scoped instance of the repository
            services.AddScoped<ISalaryPeeperRepository, SalaryPeeperRepository>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("SalaryPeeperPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
