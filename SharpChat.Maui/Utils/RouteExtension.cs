using SharpChat.Core.Services;

namespace SharpChat.Maui.Utils;

[ContentProperty(nameof(Value))]
public class RouteExtension : IMarkupExtension
{
    public Route Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return Value.ToRouteString();
    }
}
