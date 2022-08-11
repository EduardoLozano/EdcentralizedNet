using EdcentralizedNet.Business;
using EdcentralizedNet.Cache;
using EdcentralizedNet.HttpClients;
using EdcentralizedNet.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EdcentralizedNet
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
            services.AddControllersWithViews();

            //Add Redis Cache multiplexer for complex operations
            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(Configuration.GetSection("RedisCache")["ConnectionString"]);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            //Add Redis Cache as the Distributed Cache for simple operations
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("RedisCache")["ConnectionString"];
                options.InstanceName = "Edcentralized|";
            });

            //Add Http Clients
            services.AddHttpClient<EtherscanClient>();
            services.AddHttpClient<OpenseaClient>();

            //Add Repository Layer
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IAccountTokenRepository, AccountTokenRepository>();

            //Add Business Layer
            services.AddScoped<IEtherscanManager, EtherscanManager>();
            services.AddScoped<IOpenseaManager, OpenseaManager>();
            services.AddScoped<INFTManager, NFTManager>();

            //Add Caching Layer
            services.AddScoped<IApplicationCache, ApplicationCache>();
            services.AddScoped<IOpenseaCache, OpenseaCache>();
            services.AddScoped<IEtherscanCache, EtherscanCache>();
            services.AddScoped<INFTCache, NFTCache>();
            services.AddScoped<IRateLimitCache, RateLimitCache>();


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
