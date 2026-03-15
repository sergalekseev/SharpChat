using SharpChat.Core.Services;

namespace SharpChat.Maui.Services;

internal class NavigationService : INavigationService
{
    public async Task NavigateToAsync(Route route)
    {
        await Shell.Current.GoToAsync($"//{route.ToRouteString()}");
    }
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
