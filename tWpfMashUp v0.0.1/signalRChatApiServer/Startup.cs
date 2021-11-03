using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using signalRChatApiServer.Data;
using signalRChatApiServer.Hubs;
using signalRChatApiServer.Repositories;
using signalRChatApiServer.Repositories.Infra;
using signalRChatApiServer.Repositories.Repos;

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
            services.AddTransient<IChatsReposatory, ChatsReposatory>();
            services.AddTransient<IUsersReposatory, UsersReposatory>();
            services.AddTransient<IMassegesReposatory, MassegesReposatory>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TalkBackChatContext>(options => options.UseSqlServer(connectionString));
            services.AddSignalR();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, TalkBackChatContext ctx)
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/ChatHub");
                endpoints.MapControllers();
            });
        }
    }
}
