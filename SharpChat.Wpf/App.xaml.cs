using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpChat.Core.Services;
using SharpChat.Core.ViewModels;
using SharpChat.Wpf.Pages;
using SharpChat.Wpf.Services;
using System.Windows;

namespace SharpChat.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _appHost;
    private readonly MainWindow _mainWindow;

    public App()
    {
        var builder = Host.CreateDefaultBuilder();
        _mainWindow = new();

        builder.ConfigureServices((_, services) =>
        {
            // navigation frame 
            services.AddSingleton(_mainWindow.NavigationFrame);

            // services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IStartupCoordinator, StartupCoordinator>();

            // view models
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ChatViewModel>();

            // pages
            services.AddTransient<LoginPage>();
            services.AddTransient<ChatPage>();
        });

        _appHost = builder.Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            await _appHost.StartAsync();
            _mainWindow.Show();

            var startupCoordinator = _appHost.Services.GetRequiredService<IStartupCoordinator>();
            await startupCoordinator.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            if (_appHost != null)
            {
                await _appHost.StopAsync(TimeSpan.FromSeconds(5));
                _appHost.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        base.OnExit(e);
    }
}
