
using SharpChat.Core.Services;

namespace SharpChat.Maui;

public partial class App : Application
{
    private readonly IStartupCoordinator _startupCoordinator;

    public App(IStartupCoordinator startupCoordinator)
    {
        InitializeComponent();
        MainPage = new AppShell();

        _startupCoordinator = startupCoordinator;
    }

    protected override void OnStart()
    {
        base.OnStart();

        _startupCoordinator.StartAsync();
    }
}
