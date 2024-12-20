﻿namespace MYPM.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Item> _products;

    public SettingsViewModel()
    {
        _products = new ObservableCollection<Item>(
            AppData.Items
        );
    }

    [RelayCommand]
    async Task NotImplemented()
    {
        await Application.Current.MainPage.DisplayAlert("Not Implemented", "Wouldn't it be nice tho?", "Okay");
    }
}

