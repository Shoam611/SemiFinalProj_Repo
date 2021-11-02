using System;
using System.Windows;
using tWpfMashUp_v0._0._1.Sevices;
using Microsoft.Extensions.Hosting;
using tWpfMashUp_v0._0._1.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace tWpfMashUp_v0._0._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        private IHost host;
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            host = Host.CreateDefaultBuilder()
                    .ConfigureServices(ConfigServices)
                    .Build();
            await host.StartAsync();
            Start(host.Services);
        }
        private void ConfigServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            
            services.AddScoped<MainViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<ChatAppViewModel>();
            services.AddScoped<ChatThreadViewModel>();
            services.AddScoped<GameViewModel>();
            
            services.AddSingleton<StoreService>();
            services.AddScoped<SignalRListinerService>();
            services.AddScoped<AuthenticationService>();

            services.AddTransient<NavigationService>();
            services.AddTransient<MessagesService>();
            services.AddTransient<ChatsService>();
        }
        private static void Start(IServiceProvider services)
        {
            ServiceProvider = services;
            ServiceProvider.GetService<MainWindow>().Show();
        }        
        protected async override void OnExit(ExitEventArgs e)
        {
            // service.OnLogOutHandler();
            using (host) await host.StopAsync(TimeSpan.FromSeconds(2));
            //LogOut
            base.OnExit(e);
        }
    }
}

//TODO: add onExit logOutCallToServer. Done
//TODO: add On Logging in Fetching all currently connected users.
//TODO: add status changes in store and db when user logging in and out.
//TODO: when user is logged in ,user.remove? from oflline & vice versa.
//TODO: when user log off with no chat remove complitly.
//TODO: on MessageSent Inform User -> \/
////    if not on current chat, make circle green
////     else push thread update
//TODO: LogOut functionality
//TODO: Export on server: hub and repo logic to services;☻
//TODO: update Hubconnectionstring on new logging in
//TODO: start listening to hub only after logging in
