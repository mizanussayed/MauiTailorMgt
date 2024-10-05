using MauiApp1.Pages.Handheld;

namespace MauiApp1;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new AppShell();
        MainPage = new NavigationPage(new MobileLoginPage());
    }
}
