﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RACI.ASCOM.Service.Models;
using RACI.ASCOM.Service.Services;
using RACI.Data;
using ASCOM.DriverAccess;
using ASCOM.Utilities;

namespace RACI.ASCOM.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostingEnvironment env, ILoggerFactory logFactory)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddScoped<TraceLogger>();
            services.AddDbContext<RaciModel>(ServiceLifetime.Scoped);
            services.AddIdentity<RaciUser, IdentityRole>()
                .AddEntityFrameworkStores<RaciModel>()
                .AddDefaultTokenProviders();
            services.AddSingleton<DriverApiService>();
            services.AddSingleton<DriverImplementationService>();

            services.AddAscom();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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
