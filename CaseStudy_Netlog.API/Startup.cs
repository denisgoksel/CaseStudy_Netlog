using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CaseStudy_Netlog.Data.Context;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.API.Services;
using CaseStudy_Netlog.API.BackgroundServices;

namespace CaseStudy_Netlog.API
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
            // HttpClient tek sefer eklenir
            services.AddHttpClient();

            services.AddScoped<IOrderImportService, OrderImportService>();
            services.AddScoped<IDeliveryService, DeliveryService>();

            // Sadece aþaðýdaki iki servis aktif olmalý:
            services.AddHostedService<DailyIntegrationService>();   // Günlük sipariþ çekme + teslimat gönderme
            services.AddHostedService<HourlyIntegrationService>();  // Saatlik teslimat durumu güncelleme


            // DbContext konfigürasyonu
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                     b => b.MigrationsAssembly("CaseStudy_Netlog.Data")));

            // MVC Controller'lar
            services.AddControllers();

            // Swagger ayarlarý
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CaseStudy_Netlog.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaseStudy_Netlog.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
