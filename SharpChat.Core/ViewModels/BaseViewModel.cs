using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharpChat.Core.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual Task OnAppearing()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDisappearing()
    {
        return Task.CompletedTask;
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    protected void RaisePropertyChangedEvent(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
