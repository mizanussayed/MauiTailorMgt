
namespace MYPM.Pages.Handheld;

public partial class MobileLoginViewModel : ObservableObject
{
    [RelayCommand]
    private async Task Login()
    {
        if (Application.Current is not null)
           Application.Current.MainPage = new AppShell();
        else
        await ShowMessage("Error", "Something Went To Wrong");
    }

    private async Task ShowMessage(string title, string message)
    {
        _ = Application.Current?.MainPage?.Dispatcher.Dispatch(async () =>
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK").ConfigureAwait(false);
        });
        await Task.CompletedTask;
    }
}