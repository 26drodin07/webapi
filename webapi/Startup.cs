using CodeFirst;
using CodeFirst.Models;
using IdentityServer.Configuration;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Jwt;
using webapi.Models;

namespace webapi
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


            services.AddSignalR(options=>
            {
                options.EnableDetailedErrors = true;
            }            
            );
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost", "htmlpage.html:1");
                    });
            });
            services.AddDbContext<DataContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlite("Data Source=Identity.db;"));


            /*обычный identity*/
                        services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

           /* var builder = services.AddIdentity<ApplicationUser, IdentityRole>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
            identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();*/


            // services.AddIdentityServer().AddApiAuthorization<ApplicationUser,ApplicationDbContext>();


            /* var builder = services.AddIdentityServer(options =>
             {
                 options.Events.RaiseErrorEvents = true;
                 options.Events.RaiseInformationEvents = true;
                 options.Events.RaiseFailureEvents = true;
                 options.Events.RaiseSuccessEvents = true;
                 options.EmitStaticAudienceClaim = true;

                 options.Endpoints = new EndpointsOptions
                 {
                     EnableAuthorizeEndpoint = true,
                     EnableCheckSessionEndpoint = true,
                     EnableEndSessionEndpoint = true,
                     EnableUserInfoEndpoint = true,
                     EnableDiscoveryEndpoint = true,
                     EnableIntrospectionEndpoint = true,
                     EnableTokenEndpoint = true,
                     EnableTokenRevocationEndpoint = true
                 };

                 options.Authentication = new AuthenticationOptions
                 {
                     CookieLifetime = TimeSpan.FromDays(1)
                 };
             })
                 .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
                 .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                 .AddInMemoryClients(IdentityConfiguration.Clients)
                 .AddAspNetIdentity<ApplicationUser>();

             builder.AddDeveloperSigningCredential();*/
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<ChatModel>();
            services.AddScoped<MessageModel>();
            services.AddScoped<UserModel>();



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => 
                    {
                        o.RequireHttpsMetadata = false;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey = key,
                            ValidateIssuerSigningKey = true,
                        };}); 

          

          services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = "Mycookies";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.LoginPath = "/api/login";
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            } );
            services.AddControllers();
            services.AddSwaggerGen();

            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c=>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });

            
          //  app.UseIdentityServer();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
               
            });
        }





    }
}
