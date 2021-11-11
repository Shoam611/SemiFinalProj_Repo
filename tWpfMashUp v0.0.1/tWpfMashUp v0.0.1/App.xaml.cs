using System;
using System.Windows;
using tWpfMashUp_v0._0._1.Sevices;
using Microsoft.Extensions.Hosting;
using tWpfMashUp_v0._0._1.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using tWpfMashUp_v0._0._1.Assets.Components.CustomModal;

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
            services.AddScoped<ModalViewModel>();
            
            services.AddSingleton<StoreService>();
            
            services.AddScoped<AuthenticationService>();
            services.AddScoped<SignalRListenerService>();

            services.AddTransient<NavigationService>();
            services.AddTransient<MessagesService>();
            services.AddTransient<ChatsService>();
            services.AddTransient<GameLogicService>();
        }
        private static void Start(IServiceProvider services)
        {
            ServiceProvider = services;
            ServiceProvider.GetService<MainWindow>().Show();
        }        
        protected async override void OnExit(ExitEventArgs e)
        {
            using (host) await host.StopAsync(TimeSpan.FromSeconds(2));
            base.OnExit(e);
        }
    }
}
//Ergent
//TODO: Export on server: hub and repo logic to services; ☻ -> later

//TODO: if user has pending request he cannot sent another one while waiting 


//active tasks
//TODO: LogOut functionality.
