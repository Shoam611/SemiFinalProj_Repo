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
            
            services.AddScoped<AuthenticationService>();
            services.AddScoped<SignalRListenerService>();

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
            using (host) await host.StopAsync(TimeSpan.FromSeconds(2));
            base.OnExit(e);
        }
    }
}
//Ergent
//TODO: Export on server: hub and repo logic to services; ☻ -> later

//not complited:
//TODO: when user is logged in ,user.remove? from oflline & vice versa. Halfy done
//TODO: on MessageSent Inform User -> \/
////    if not on current chat, make circle green 
////     else push thread update -> Done

//active tasks
//TODO: LogOut functionality.
//TODO: Chats:When user is Selected
    //chat will be created if not exist.
    //fetched chat will be added to the store
    //chatthread viewModel will be notified and update Ui
//TODO: Add Validation to registraion.
        //no empty strings or less than 2 -> Done
        //no duplicate usernames -> Done
        //add matching error massage to user -> Done
//TODO: Add Validation to messages sending
        //no empty / spaces or null message content!
