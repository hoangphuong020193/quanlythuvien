using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Library.Data.Services;
using Microsoft.AspNetCore.Http;
using Library.ApiFramework.Authorization;
using DryIoc;
using Library.ApiFramework.IoCRegistrar;
using DryIoc.Microsoft.DependencyInjection;
using System;
using AutoMapper;
using Library.ApiFramework.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Library.ApiFramework.AppSetting.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Library.ApiFramework.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Library.ApiFramework.AppConfig;
using Microsoft.AspNetCore.Identity;
using Library.ApiFramework.ConfigurationExtensions;

namespace Library.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            Mapper.Initialize(configuration => configuration.AddProfile<MappingProfile>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCaching();

            services.AddMvc(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                cfg.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(opt =>
            {
                opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            var jwtSettingOptions = Configuration.GetSection("JwtOptions");

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtSettingOptions["Issuer"];
                options.Audience = jwtSettingOptions["Audience"];
                options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingOptions["SecretKey"])), SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtSettingOptions["Issuer"],
                    ValidAudience = jwtSettingOptions["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingOptions["SecretKey"])) //Secret
                };
            });

            services.AddSingleton(ctx => Configuration);
            string connectionString = Configuration.GetDbConnectionString();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Server=.\SQLEXPRESS;Database=library;Trusted_Connection=True;"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins(Configuration.GetAllowOrigins())
                        .AllowAnyHeader().AllowAnyMethod());
            });

            services.ConfigurePermissions();
            services.AddMvc();

            return new Container()
                .WithDependencyInjectionAdapter(services, throwIfUnresolved: type => type.Name.EndsWith("Controller"))
                .ConfigureServiceProvider<CompositionRoot>();
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
                app.UseExceptionHandler("/error");
            }
            app.UseAuthentication();
            app.UseResponseCaching();
            app.UseCors("AllowSpecificOrigin");
            app.UseMvc();
        }
    }
}
