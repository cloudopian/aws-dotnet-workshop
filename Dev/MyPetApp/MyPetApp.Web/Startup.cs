using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyPetApp.Config;
using System.Security.Claims;
using Amazon;
using MyPetApp.Security;
using Amazon.SecurityToken;

namespace MyPetApp.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            AWSCredentials creds = ConfigManager.GetCredentails();
            RegionEndpoint region = ConfigManager.GetRegion();
            
            string userPoolid = "ap-southeast-2_U634533B";
            string clientId = "4baarnf1111111111111111111";

            IAmazonCognitoIdentityProvider identityProvider = new AmazonCognitoIdentityProviderClient(creds, region);
     
            CognitoUserPool userPool = new CognitoUserPool(userPoolid, clientId, identityProvider);
            AWSOptions awsOptions = new AWSOptions();
            awsOptions.Region = Amazon.RegionEndpoint.APSoutheast2;
            awsOptions.Credentials = creds;
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonCognitoIdentityProvider>();
            services.AddAWSService<IAmazonSecurityTokenService>();

            services.AddSingleton<CognitoUserPool>(userPool);
            services.AddScoped<ITenantSecurity, TenantSecurity>();

            services.AddCognitoIdentity();

            services.AddRazorPages();
            services.AddAuthentication();
            services.AddTransient<ClaimsPrincipal>(s =>s.GetService<IHttpContextAccessor>().HttpContext.User);

            services.ConfigureApplicationCookie(options => { 
             options.LoginPath = new PathString("/Identity/Account/Login");
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
 
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
