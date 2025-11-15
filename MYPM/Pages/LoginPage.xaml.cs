#if ANDROID
using MYPM.Platforms.Android;
#endif

namespace MYPM.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetupStatusBar();
    }

    private void SetupStatusBar()
    {
        try
        {
            if (Application.Current?.MainPage?.Window is Window window &&
                Application.Current?.Resources.TryGetValue("DarkBg1Transparent", out var bgColor) == true &&
                bgColor is Color statusBarColor)
            {
                window.SetStatusBarColor(statusBarColor, darkContent: true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error setting status bar: {ex.Message}");
        }
    }
}
