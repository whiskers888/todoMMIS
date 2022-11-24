using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using todoMMIS.Contexts;
using todoMMIS.Managers;

namespace todoMMIS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddMvc(mvc => { mvc.EnableEndpointRouting = false; });
            services.AddSingleton<ApplicationContext>();
            
        }  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=App}/{action=Index}/{id?}");

                routes.MapRoute("NotFound", "{*url}",
                    new { controller = "App", action = "RedirectToMain" });
            });
        }
    }
}
