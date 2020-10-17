using BlossomProduct.Core.EFContext;
using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.Repo;
using BlossomProduct.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            services.AddAuthentication()
                .AddGoogle( options =>
                {
                    options.ClientId = "900514984451-s4t5le4if95hpakfpcicrnpk6dp2lgeh.apps.googleusercontent.com";
                    options.ClientSecret = "Ghy8Ob30FoOJUvdVojMk3SK1";
                } )
                .AddFacebook( options =>
                 {
                     options.AppId = "1171369249932532";
                     options.AppSecret = "fd85f519342ca52c2ef2cbd23ffd77c2";
                 } );

            services.ConfigureApplicationCookie( options =>
            {
                options.AccessDeniedPath = new PathString( "/Administration/AccessDenied" );
            } );

            services.AddAuthorization( options =>
            {
                options.AddPolicy( "DeleteRolePolicy",
                    policy => policy.RequireClaim( "Delete Role" ) );

                options.AddPolicy( "EditRolePolicy", policy =>
            policy.AddRequirements( new ManageAdminRolesAndClaimsRequirement() ) );

                options.AddPolicy( "AdminRolePolicy",
                    policy => policy.RequireRole( "Admin" ) );
            } );

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

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
