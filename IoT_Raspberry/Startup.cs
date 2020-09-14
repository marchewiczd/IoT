using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IoT_RaspberryServer.Data;
using Radzen;
using System;
using System.Threading;
using System.Diagnostics;

namespace IoT_RaspberryServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Debug Mode!");
                LoadTempData();
            }

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            
            // Load secrets
            AppSettings.OpenWeatherApiKey = Configuration["apiKeys:openWeatherApiKey"];
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private void LoadTempData()
        {
            for (int i = 0; i < 10; i++)
            {
                IoT_RaspberryServer.Data.Data.Sprinklers.Add(new Sprinkler
                {
                    Description = "Im a sprinkler test123",
                    GpioPin = 123,
                    LastSuccessfulSprinkle = new DateTime(2020, 9, 14, 14, 56, 12),
                    SkipNextSprinkle = false,
                    SprinkleStatus = false
                });

                IoT_RaspberryServer.Data.Data.Sprinklers[i].SprinkleTimeDict.Add(new DateTime(2020, 9, 14, 14, 56, 12), 13);
                IoT_RaspberryServer.Data.Data.Sprinklers[i].SprinkleTimeDict.Add(new DateTime(2020, 9, 14, 15, 15, 12), 13);
                Thread.Sleep(10);
            }
        }
    }
}
