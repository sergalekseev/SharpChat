namespace SharpChat.Core.Services
{
    public enum Route
    {
        Login,
        Chat
    }

    public static class RouteExtensions
    {
        public static string ToRouteString(this Route route)
        {
            return route.ToString().ToLowerInvariant();
        }
    }
}
