using BusinessObjects.Mapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Repositories.IRepositories;
using Repositories.Repositories;
using System;
using System.IO;
using System.Reflection;
using ViralMusicAPI.Handler;

namespace ViralMusicAPI
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
            // Controller
            services.AddControllers();

            // Auto mapper
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // Database
            services.AddDbContext<ViralMusicContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // DI
            services.AddScoped<IUserRepository, UserRepository>();

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Viral Music API",
                    Description = "An ASP.NET Core Web API for managing Viral Music",
                    Contact = new OpenApiContact
                    {
                        Name = "tienhuynh-tn",
                        Url = new Uri("https://github.com/tienhuynh-tn")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "The GNU General Public License v3.0",
                        Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.html")
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ViralMusicAPI v1"));
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ViralMusicAPI v1"));

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}