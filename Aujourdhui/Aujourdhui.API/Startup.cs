using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aujourdhui.API
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
            services.AddControllers();

            var authSection = Configuration.GetSection("Authentication");
            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        var google = authSection.GetSection("Google");
                        options.ClientId = google["ClientId"];
                        options.ClientSecret = google["ClientSecret"];
                    })
                    .AddFacebook(options =>
                    {
                        var facebook = authSection.GetSection("Facebook");
                        options.AppId = facebook["AppId"];
                        options.AppSecret = facebook["AppSecret"];
                    })
                    .AddTwitter(options => {
                        var twitter = authSection.GetSection("Twitter");
                        options.ConsumerKey = twitter["ConsumerKey"];
                        options.ConsumerSecret = twitter["ConsumerSecret"];
                        options.RetrieveUserDetails = true;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
