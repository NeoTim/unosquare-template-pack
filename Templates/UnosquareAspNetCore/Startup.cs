using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan.AspNetCore;

namespace UnosquareAspNetCore
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup a basic stores for Identity and remove Cookies redirects to /Login
            services.AddIdentity<BasicUserStore, BasicRoleStore>(options => options.SetupCookies()).AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Redirect anything without extension to index.html
            app.UseFallback();
            // Response an exception as JSON at error
            app.UseJsonExceptionHandler();

            app.UseIdentity();

            // Use the bearer token provider and check Admin and Passw.ord as valid credentials
            app.UseBearerTokenProvider(new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SymmetricSecurityKey"])),

                ValidateIssuer = true,
                ValidIssuer = "UnosquareAspNetCore",

                ValidateAudience = true,
                ValidAudience = "Unosquare",

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            }, (username, password, grantType, clientId) =>
            {
                // TODO: Implement your authentication process here
                if (username != "Admin" || password != "Pass.word")
                    return Task.FromResult<ClaimsIdentity>(null);

                var claim = new ClaimsIdentity("Bearer");
                claim.AddClaim(new Claim(ClaimTypes.Name, username));

                return Task.FromResult(claim);
            }, (identity, obj) =>
            {
                // This action is optional to add properties to the Bearer token
                obj["test"] = "OK";
                return Task.FromResult(obj);
            }, 
            forceHttps: false); // TODO: We recommend use HTTPS but the default behavior is off

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}