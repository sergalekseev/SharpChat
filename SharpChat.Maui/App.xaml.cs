using SharpChat.Core.Services;

namespace SharpChat.Maui
{
    public partial class App : Application
    {
        private readonly IStartupCoordinator _startupCoordinator;

        public App(IStartupCoordinator startupCoordinator)
        {
            InitializeComponent();
            _startupCoordinator = startupCoordinator;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            base.OnStart();

            _startupCoordinator.StartAsync();
        }

    }
}