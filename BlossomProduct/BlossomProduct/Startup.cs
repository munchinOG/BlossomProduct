﻿using BlossomProduct.Core.EFContext;
using BlossomProduct.Core.Models.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddControllersWithViews();
            services.AddControllers( options => options.EnableEndpointRouting = false );
            services.AddMvc();

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

            app.UseRouting();

            app.UseAuthorization();

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
