using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using tWpfMashUp_v0._0._1.MVVM.ViewModels;
using tWpfMashUp_v0._0._1.Sevices;

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
            
            services.AddTransient<NavigationService>();
        }
        private void Start(IServiceProvider services)
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
