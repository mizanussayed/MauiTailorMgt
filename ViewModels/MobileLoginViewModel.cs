
namespace MYPM.ViewModels;

public partial class MobileLoginViewModel : ObservableObject
{
    [RelayCommand]
    private static async Task Login()
    {
        if (Application.Current is not null && Application.Current.Windows.Count > 0)
            Application.Current.Windows[0].Page = new AppShell();
        else
            await ShowMessage("Error", "Something Went To Wrong");
    }

    private static async Task ShowMessage(string title, string message)
    {
        if (Application.Current?.Windows.Count > 0)
        {
            var page = Application.Current.Windows[0].Page;
            if (page != null)
            {
                _ = Application.Current.Windows[0].Dispatcher.Dispatch(async () =>
                {
                    await page.DisplayAlert(title, message, "OK").ConfigureAwait(false);
                });
            }
        }
        await Task.CompletedTask;
    }
}
