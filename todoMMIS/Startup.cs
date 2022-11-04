using todoMMIS.Contexts;

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
