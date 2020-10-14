using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CreditCards.Infrastructure;
using CreditCards.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CreditCards
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        public IHostEnvironment _environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_configuration.GetConnectionString("TestingDatabase")));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddScoped<ICreditCardApplicationRepository, EntityFrameworkCreditCardApplicationRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, AppDbContext dbContext)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.EnsureCreated();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
