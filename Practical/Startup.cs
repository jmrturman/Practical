using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using MainService.Interfaces;
using MainService.Services;
using ReverseDNS.Interfaces;
using ReverseDNS.Services;
using GeoIP.Interfaces;
using GeoIP.Services;
using PingIP.Interfaces;
using PingIP.Services;
using RDAP.Interfaces;
using RDAP.Services;
using PracticalValidation.Interfaces;
using PracticalValidation.Services;

namespace Practical
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
            _ = services.AddScoped<IPracticalService, PracticalService>();
            services.AddScoped<IReverseDNSService, ReverseDNSService>();
            services.AddScoped<IGeoIPService, GeoIPService>();
            services.AddScoped<IPingService, PingService>();
            services.AddScoped<IRDAPService, RDAPService>();

            services.AddTransient<IPracticalValidationService, PracticalValidationService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpClient("APIClient", client =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
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
