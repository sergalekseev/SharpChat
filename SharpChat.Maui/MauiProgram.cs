using Microsoft.Extensions.Logging;
using SharpChat.Core.Services;
using SharpChat.Core.ViewModels;
using SharpChat.Maui.Pages;
using SharpChat.Maui.Services;

namespace SharpChat.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // services
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IStartupCoordinator, StartupCoordinator>();

        // view models
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ChatViewModel>();

        // pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ChatPage>();

        return builder.Build();
    }
}
