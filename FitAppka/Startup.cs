using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Repository.RepIfaceImpl;
using FitAppka.Service;
using FitAppka.Service.ServiceImpl;
using FitAppka.Service.ServicesImpl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FitAppka
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
            
            services.AddControllersWithViews();
            services.AddDbContext<FitAppContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/Login";
            });

            services.AddHttpContextAccessor();
            ServicesAddScoped(services);
            RepositoriesAddScoped(services);
        }

        private void ServicesAddScoped(IServiceCollection services)
        {
            services.AddScoped<IDietaryTargetsService, DietaryTargetsServiceImpl>();
            services.AddScoped<ISettingsService, SettingsServiceImpl>();
            services.AddScoped<IStrengthTrainingService, StrengthTrainingServiceImpl>();
            services.AddScoped<ICardioTrainingService, CardioTrainingServiceImpl>();
            services.AddScoped<IHomePageService, HomePageServiceImpl>();
            services.AddScoped<IFindDayService, FindDayServiceImpl>();
            services.AddScoped<IClientManageService, ClientManageServiceImpl>();
            services.AddScoped<IProductManageService, ProductManageServiceImpl>();
            services.AddScoped<IGoalsService, GoalsServiceImpl>();
            services.AddScoped<IDayManageService, DayServiceImpl>();
        }

        private void RepositoriesAddScoped(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, SQLProductRepository>();
            services.AddScoped<IMealRepository, SQLMealRepository>();
            services.AddScoped<IDayRepository, SQLDayRepository>();
            services.AddScoped<IClientRepository, SQLClientRepository>();
            services.AddScoped<IStrengthTrainingTypeRepository, SQLStrengthTrainingTypeRepository>();
            services.AddScoped<IStrengthTrainingRepository, SQLStrengthTrainingRepository>();
            services.AddScoped<ICardioTrainingTypeRepository, SQLCardioTrainingTypeRepository>();
            services.AddScoped<ICardioTrainingRepository, SQLCardioTrainingRepository>();
            services.AddScoped<IWeightMeasurementRepository, SQLWeightMeasurementRepository>();
            services.AddScoped<IFatMeasurementRepository, SQLFatMeasurementRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*else
            {
                app.UseStatusCodePagesWithReExecute()
            }*/
      
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}
