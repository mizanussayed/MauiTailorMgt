using MYPM.ViewModels;

namespace MYPM.Pages;

public partial class GalleryListPage : ContentPage
{
    private GalleryViewModel _viewModel;
    public GalleryListPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new GalleryViewModel();
    }


    private async void OnAddImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            using var stream = await result.OpenReadAsync();
            await _viewModel.UploadImageAsync(stream, result.FileName);
        }
    }
}