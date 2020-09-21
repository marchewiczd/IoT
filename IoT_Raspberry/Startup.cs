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
using System.IO;
using System.Linq;
using IoT_RaspberryServer.Services;

namespace IoT_RaspberryServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            if (Debugger.IsAttached)
            {
                LoadTempData();

                AppSettings.OpenWeatherApiKey = configuration["apiKeys:openWeatherApiKey"];
            }
            else
            {
                AppSettings.OpenWeatherApiKey = File.ReadLines(".apiKey").First();
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Radzen services
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();

            // Project services
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<SprinklerService>();
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
                Data.Data.Sprinklers.Add(new Sprinkler
                {
                    Description = "Im a sprinkler test123",
                    GpioPin = 123,
                    LastSuccessfulSprinkle = new DateTime(2020, 9, 14, 14, 56, 12),
                    SkipNextSprinkle = false,
                    SprinkleStatus = false
                });

                Data.Data.Sprinklers[i].SprinkleTimeList.Add(new SprinklerDateTime
                {
                    WateringDateTime = new DateTime(2020, 9, 14, 14, 56, 12),
                    WateringDuration = 13
                });

                Data.Data.Sprinklers[i].SprinkleTimeList.Add(new SprinklerDateTime
                {
                    WateringDateTime = new DateTime(2020, 9, 14, 14, 56, 12),
                    WateringDuration = 13
                });

                Thread.Sleep(10);
            }
        }
    }
}
