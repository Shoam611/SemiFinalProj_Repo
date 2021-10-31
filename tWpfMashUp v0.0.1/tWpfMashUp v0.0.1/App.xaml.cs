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
            
            services.AddScoped<SignalRListinerService>();
            services.AddSingleton<StoreService>();

            services.AddScoped<AuthenticationService>();
            services.AddTransient<NavigationService>();
            services.AddTransient<MessagesService>();
            services.AddTransient<ChatsService>();
        }
        private void Start(IServiceProvider services)
        {
            ServiceProvider = services;
            ServiceProvider.GetService<MainWindow>().Show();
        }
        protected async override void OnExit(ExitEventArgs e)
        {
            using (host) await host.StopAsync(TimeSpan.FromSeconds(2));
            //LogOut
            base.OnExit(e);
        }
    }
}

//add on exit logOutCallToServer
//add On Logging in Fetching all currently connected users
//add status changes in store and db when user logging in and out
