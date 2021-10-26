using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using signalRChatApiServer.Data;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Repositories;

namespace signalRChatApiServer
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository, MainRepository>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TalkBackChatContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
            services.AddSignalR();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TalkBackChatContext ctx)
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllers();
            });
        }
    }
}
