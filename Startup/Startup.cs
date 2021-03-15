using AutoMapper;
using AWTS.BLL.AutoMapperConfiguration;
using AWTS.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;

namespace AwtsAPI.Startup
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            var builder = new ConfigurationBuilder()
               .SetBasePath(environment.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMvcCore(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.EnableEndpointRouting = false;
            })
           .AddXmlSerializerFormatters()
           .AddApiExplorer()
           .AddDataAnnotations()
           .AddCors();

            //Db context
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<AWTSContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:AWTSConnection"], sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                });
            });

            //Swagger config
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AwtsAPI.API", Version = "v1" });
            });

            //Add Automapper config
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddHttpClient();
            ServicesConfigurer.Configure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                .WithOrigins(Configuration.GetSection("Cors:Origins").Get<string[]>())
                .WithHeaders("X-Requested-With")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An exception fault happened. Try again later.");
                });
            });

            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AwtsAPI.API " + Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion);
                c.DocumentTitle = "AwtsAPI.API Env: " + Environment.EnvironmentName;
            });
            app.UseMvc();
        }
    }
}
