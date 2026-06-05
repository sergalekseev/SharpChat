using SharpChat.Core.Services;
using System.Net.Http.Headers;

namespace SharpChat.Maui.Services;

internal class AuthDelegatingHandler : DelegatingHandler
{
    IAuthService _authService;
    INavigationService _navigationService;

    public AuthDelegatingHandler(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _authService.AuthToken;
        Console.WriteLine($"access token is {token}");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized)
        {
            await MainThread.InvokeOnMainThreadAsync(async () => await _navigationService.NavigateToAsync(Route.Login));
        }

        return response;
    }
}

