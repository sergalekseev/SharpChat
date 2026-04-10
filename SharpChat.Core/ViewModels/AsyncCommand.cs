using System.Windows.Input;

namespace SharpChat.Core.ViewModels;

public class AsyncCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;

    public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public async void Execute(object? parameter)
    {
        try
        {
            await _execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Command execution failed with exception: {ex.GetType().Name}, message: {ex.Message}");
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
