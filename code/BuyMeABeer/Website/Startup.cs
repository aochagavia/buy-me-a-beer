using Domain.Integration;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Website.Auth;
using Website.Database;
using Website.Integration;
using Website.Options;
using Website.Repositories;

namespace Website
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
            services.AddCookieBasedAuth0Authentication(Configuration);
            services.AddApplicationInsightsTelemetry();
            services.AddControllersWithViews();
            services.AddDbContext<WebsiteDbContext>(options =>
                options.UseSqlServer(Configuration["Data:DbContext:ConnectionString"]));
            services.AddOptions<StripeOptions>()
                .Bind(Configuration.GetSection("Stripe"));
            services.AddOptions<DeploymentOptions>()
                .Bind(Configuration.GetSection("Deployment"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            services
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IPaymentRepository, PaymentRepository>()
                .AddScoped<IBeerProductRepository, BeerProductRepository>()
                .AddScoped<IStripeSessionService, StripeSessionService>()
                .AddScoped<CommentCreationService>()
                .AddScoped<BeerOrderService>()
                .AddScoped<PaymentService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WebsiteDbContext>();
                context.Database.Migrate();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}