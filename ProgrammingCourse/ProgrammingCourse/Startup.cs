using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProgrammingCourse.Configurations;
using ProgrammingCourse.Middlewares;
using ProgrammingCourse.Models;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCourse
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
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddCors();
            services.AddDbContext<ProgrammingCourseDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ProgrammingCourseDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<RefreshTokenRepository>();
            services.AddScoped<LectureRepository>();
            services.AddScoped<StatusRepository>();
            services.AddScoped<CategoryTypeRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<CourseRepository>();
            services.AddScoped<WatchListRepository>();
            services.AddScoped<StudentCourseRepository>();
            services.AddScoped<FeedbackRepository>();
            services.AddScoped<ViewRepository>();
            services.AddScoped<UserRepository>();

            services.AddScoped<CategoryTypeService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<CourseService>();



            //configure strongly typed settings objects
            var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");
            services.Configure<JwtBearerTokenSettings>(jwtSection);
            var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtBearerTokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtBearerTokenSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if(context.HttpContext.Items["accessToken"] != null)
                            {
                                context.Token = context.HttpContext.Items["accessToken"].ToString();
                            }
                            //context.Token = context.Request.Cookies["accessToken"];

                            //if (context.HttpContext.Items["accessToken"].ToString() != "")
                            //{
                            //    context.Token = context.HttpContext.Items["accessToken"].ToString();
                            //}
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProgrammingCourse", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProgrammingCourse v1");
                    c.RoutePrefix = string.Empty;
                });

                app.UseStaticFiles();

                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                    RequestPath = "/StaticFiles",
                    EnableDefaultFiles = true
                });
            }


            //Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<TokenVerificationMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
