using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASCOM.WebService.Models;
using ASCOM.WebService.Services;
using RACI.Data;
using Microsoft.Extensions.Logging;
using ASCOM.DriverAccess;

namespace ASCOM.WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory logFactory)
        {
            Configuration = configuration;
            localLogger = logFactory ?? new LoggerFactory();
#if DEBUG
            localLogger
                .AddDebug(LogLevel.Debug)
                .AddConsole(LogLevel.Debug);
#endif
        }

        private ILoggerFactory localLogger { get; set; }

        private void LoadAscom(IServiceCollection services=null,ILogger log=null)
        {
            log = log ?? localLogger.CreateLogger("ASCOMDriverLoad");
            List<Type> driverTypes = new List<Type>();

            try { driverTypes = DriverLoader.DriverTypes.ToList(); }
            catch (Exception ex)
            {
                log.LogCritical("An error occured loading the ASCOM drivers");
                log.LogCritical($"{ex.GetType().FullName}: {ex.Message}");
                log.LogCritical($"Details:");
                log.LogCritical($"{ex}");
                throw;
            }
            log.LogInformation($"Loaded ASCOM Drivers:");
            List<string> codeBaseList = driverTypes.Select(t => t.Assembly.CodeBase).Distinct().ToList();
            foreach (string cb in codeBaseList.OrderBy(t => t))
            {
                log.LogInformation($"[{cb}]");
                List<string> cbDrivers = driverTypes.Where(t => t.Assembly.CodeBase == cb).Select(t => t.FullName).ToList();
                foreach (string driverName in cbDrivers.OrderBy(t => t))
                    log.LogInformation($"\t{driverName}");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string ascomCS= Configuration.GetConnectionString("AscomConnection") ??
                Configuration.GetConnectionString("DefaultConnection");
            if (String.IsNullOrWhiteSpace(ascomCS))
                services.AddDbContext<RaciModel>();
            else 
                services.AddDbContext<RaciModel>(options => options.UseSqlite(ascomCS));
            

            services.AddSingleton(typeof(SystemHelper));
            services.AddTransient<IEmailSender, EmailSender>();
            //services.AddTransient<IRaciUser, RaciUser>();
            services.AddIdentity<RaciUser, IdentityRole>()
                .AddEntityFrameworkStores<RaciModel>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
