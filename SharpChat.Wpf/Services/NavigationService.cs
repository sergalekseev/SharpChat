using Microsoft.Extensions.DependencyInjection;
using SharpChat.Core.Services;
using SharpChat.Wpf.Pages;
using System.Windows;

namespace SharpChat.Wpf.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly NavigationFrame _navigationFrame;
        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<Route, Type> _routeRegistry = new()
        {
            { Route.Login, typeof(LoginPage) },
            { Route.Chat, typeof(ChatPage) },
        };

        public NavigationService(NavigationFrame navigationFrame, IServiceProvider serviceProvider)
        {
            _navigationFrame = navigationFrame;
            _serviceProvider = serviceProvider;
        }

        public Task NavigateToAsync(Route route)
        {
            if (!_routeRegistry.TryGetValue(route, out var viewType))
            {
                throw new InvalidOperationException($"Route {route} is not registered");
            }

            var view = (FrameworkElement)_serviceProvider.GetRequiredService(viewType);
            _navigationFrame.Navigate(view);

            return Task.CompletedTask;
        }
        public Task GoBackAsync()
        {
            if (_navigationFrame.CanGoBack)
            {
                _navigationFrame.GoBack();
            }

            return Task.CompletedTask;
        }
    }
}
