
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiAutors;
//using WebApiAutors.Context;
using WebApiAutors.Controllers;
using WebApiAutors.Filters;
using WebApiAutors.Middlewares;
//using WebApiAutors.Helpers;
using WebApiAutors.Services;

namespace WebApiAutors
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
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //custom service interface
            services.AddTransient<IService, ServiceA>();
            services.AddTransient<ServiceTransient>();
            services.AddScoped<ServiceScoped>();
            services.AddSingleton<ServiceSingleton>();
            services.AddTransient<ActionFilter>();

            services.AddHostedService<WriteInFile>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseMiddleware<LogHTTPResponseMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "WebApiAutors v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseResponseCaching(); //cache filter
           // app.UseAuthentication();//OJO
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    } 
}