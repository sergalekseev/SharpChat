namespace SharpChat.Core.Services;

public interface IMainThread
{
    void BeginInvokeOnMainThreadAsync(Action action);
    Task InvokeOnMainThreadAsync(Func<Task> action);
}

