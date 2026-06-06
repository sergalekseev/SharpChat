using SharpChat.Core.ViewModels;

namespace SharpChat.Maui.Pages;

public class BasePage : ContentPage
{
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BaseViewModel vm)
        {
            try
            {
                await vm.OnAppearing();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected issue ({ex}) occurs: {ex.Message}");
            }
        }
    }

    protected override async void OnDisappearing()
    {
        if (BindingContext is BaseViewModel vm)
        {
            try
            {
                await vm.OnDisappearing();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected issue ({ex}) occurs: {ex.Message}");
            }
        }

        base.OnDisappearing();
    }
}
