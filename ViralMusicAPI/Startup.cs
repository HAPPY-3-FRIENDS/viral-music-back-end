using BusinessObjects.Mapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.IRepositories;
using Repositories.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ViralMusicAPI.Filter;
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
            services.AddControllersWithViews().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Auto mapper
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // Database
            services.AddDbContext<ViralMusicContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // DI
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ITrackRepository, TrackRepository>();
            services.AddScoped<ITrackInPlaylistRepository, TrackInPlaylistRepository>();
            services.AddScoped<ITrackArtistRepository, TrackArtistRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();

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
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.OperationFilter<AuthOperationFilter>();

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ViralMusicAPI v1.0.0"));
            }

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}