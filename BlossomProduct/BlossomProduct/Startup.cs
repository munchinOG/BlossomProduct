using BlossomProduct.Core.EFContext;
using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlossomProduct
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddDbContextPool<BlossomDbContext>( options =>
                 options.UseSqlServer( Configuration.GetConnectionString( "BlossomDBConnection" ) ) );

            services.AddIdentity<ApplicationUser, IdentityRole>( options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = true;
                } ).AddEntityFrameworkStores<BlossomDbContext>();


            services.AddControllersWithViews();
            services.AddControllers( options => options.EnableEndpointRouting = false );
            services.AddMvc( config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add( new AuthorizeFilter( policy ) );
            } );

            services.AddScoped<IProductRepository, SqlProductRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Error/" );
                app.UseStatusCodePagesWithReExecute( "/Error/{0}" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            //app.UseAuthorization();

            app.UseMvc( routes =>
            {
                routes.MapRoute( "default", "{controller}/{action}/{id}" );
            } );

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllerRoute( "default", "{controller=home}/{action=index}/{id?}" );
            } );

            //app.UseEndpoints( endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller=Home}/{action=Index}/{id?}" );
            // } );
        }
    }
}
