using SharpChat.Core.Services;

namespace SharpChat.Maui.Services;

internal class MainThreadService : IMainThread
{
    public void BeginInvokeOnMainThreadAsync(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }

    public Task InvokeOnMainThreadAsync(Func<Task> action)
    {
        return MainThread.InvokeOnMainThreadAsync(action);
    }
}

