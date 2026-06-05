using Microsoft.Extensions.Logging;
using SharpChat.Core.Services;
using SharpChat.Core.Services.ApiClients;
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

        // api clients
        var serverAddress = new Uri("http://localhost:5153/");
        builder.Services.AddTransient<AuthDelegatingHandler>();

        builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
        {
            client.BaseAddress = serverAddress;
        });

        builder.Services.AddHttpClient<IChatsApiClient, ChatsApiClient>(client =>
        {
            client.BaseAddress = serverAddress;
        }).AddHttpMessageHandler<AuthDelegatingHandler>();

        builder.Services.AddHttpClient<IMessagesApiClient, MessagesApiClient>(client =>
        {
            client.BaseAddress = serverAddress;
        }).AddHttpMessageHandler<AuthDelegatingHandler>();

        return builder.Build();
    }
}
