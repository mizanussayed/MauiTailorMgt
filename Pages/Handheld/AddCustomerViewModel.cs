using MYPM.Messages;

namespace MYPM.Pages.Handheld;

public partial class AddCustomerViewModel : ObservableObject
{
    [ObservableProperty]
    Customer customer = new();

    //[ObservableProperty]
    //string category = ItemCategory.Noodles.ToString();

    [RelayCommand]
    private void Save()
    {
        //ItemCategory cat = (ItemCategory)Enum.Parse(typeof(ItemCategory), Category);
        //Item.Category = cat;
        //AppData.Items.Add(Item);

        WeakReferenceMessenger.Default.Send<AddProductMessage>(new AddProductMessage(false));
    }

    [RelayCommand]
    Task Cancel()
    {
        WeakReferenceMessenger.Default.Send<AddProductMessage>(new AddProductMessage(false));

        return Task.CompletedTask;
    }
}


