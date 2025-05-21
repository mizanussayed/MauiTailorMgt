namespace MYPM.Pages;

public partial class GalleryListPage : ContentPage
{
    public GalleryListPage()
    {
        InitializeComponent();
    }

    private async void OnNavigating(object sender, WebNavigatingEventArgs e)
    {
        e.Cancel = true;
        await Launcher.Default.OpenAsync(new Uri("https://www.facebook.com/YousufArabianTailors"));
    }
}