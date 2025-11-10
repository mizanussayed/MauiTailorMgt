using Microsoft.Maui.Controls.Shapes;
using MYPM.Models;
using MYPM.ViewModels;

namespace MYPM.Pages;

[QueryProperty(nameof(Order), "Order")]
public partial class EditOrderPage : ContentPage
{
    private int childFormIdCounter = 0;
    private readonly EditOrderPageViewModel vm;

    public EditOrderPage(EditOrderPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = vm = viewModel;
    }

    public NewOrderModel? Order
    {
        get => vm.Order;
        set
        {
            if (value is not null)
            {
                vm.Order = value;
                DynamicFormFields.Children.Clear();
                AddOrEnsureMeasurementSection();
            }
        }
    }

    private void OnAddMeasurementType(object sender, EventArgs e)
    {
        var orderFor = vm?.Order.OrderFor?.ToLower();
        if (string.IsNullOrWhiteSpace(orderFor)) return;
        switch (orderFor)
        {
            case "arabian":
                vm!.Order.ArabianOrders.Add(new ArabianOrder());
                break;
            case "panjabi":
                vm!.Order.PanjabiOrders.Add(new PanjabiOrder());
                break;
            case "selowar":
            case "selower":
                vm!.Order.SelowerOrders.Add(new SelowerOrder());
                break;
        }
        AddOrEnsureMeasurementSection(true);
        vm!.RefreshComputed();
    }

    private void AddOrEnsureMeasurementSection(bool forceRecreate = false)
    {
        var selected = vm!.Order.OrderFor?.ToLower();
        if (forceRecreate) DynamicFormFields.Children.Clear();

        if (!string.IsNullOrWhiteSpace(selected))
        {
            if (selected == "arabian" && vm!.Order.ArabianOrders.Count == 0) vm.Order.ArabianOrders.Add(new ArabianOrder());
            else if (selected == "panjabi" && vm!.Order.PanjabiOrders.Count == 0) vm.Order.PanjabiOrders.Add(new PanjabiOrder());
            else if ((selected == "selowar" || selected == "selower") && vm!.Order.SelowerOrders.Count == 0) vm.Order.SelowerOrders.Add(new SelowerOrder());
        }

        RenderTypeSections(
            vm.Order.ArabianOrders.Count,
            i => $"Order.ArabianOrders[{i}]",
            i => $"এরাবিয়ান এর মাপ #{i + 1}",
            prefix => CreateArabianGrid(prefix),
            i =>
            {
                if (i >= 0 && i < vm.Order.ArabianOrders.Count)
                {
                    var item = vm.Order.ArabianOrders.ElementAt(i);
                    vm.Order.ArabianOrders.Remove(item);
                }
                AddOrEnsureMeasurementSection(true);
                vm.RefreshComputed();
            }
        );

        RenderTypeSections(
            vm.Order.PanjabiOrders.Count,
            i => $"Order.PanjabiOrders[{i}]",
            i => $"পাঞ্জাবির মাপ #{i + 1}",
            prefix => CreatePanjabiGrid(prefix),
            i =>
            {
                if (i >= 0 && i < vm.Order.PanjabiOrders.Count)
                {
                    var item = vm.Order.PanjabiOrders.ElementAt(i);
                    vm.Order.PanjabiOrders.Remove(item);
                }
                AddOrEnsureMeasurementSection(true);
                vm.RefreshComputed();
            }
        );

        RenderTypeSections(
            vm.Order.SelowerOrders.Count,
            i => $"Order.SelowerOrders[{i}]",
            i => $"সেলোয়ার এর মাপ #{i + 1}",
            prefix => CreateSelowarGrid(prefix),
            i =>
            {
                if (i >= 0 && i < vm.Order.SelowerOrders.Count)
                {
                    var item = vm.Order.SelowerOrders.ElementAt(i);
                    vm.Order.SelowerOrders.Remove(item);
                }
                AddOrEnsureMeasurementSection(true);
                vm.RefreshComputed();
            }
        );
    }

    private void RenderTypeSections(int count, Func<int, string> bindPrefixFactory, Func<int, string> headerFactory, Func<string, Grid> gridFactory, Action<int> onRemove)
    {
        for (int i = 0; i < count; i++)
        {
            var index = i; // capture per-iteration
            var prefix = bindPrefixFactory(index);
            var header = headerFactory(index);
            AddMeasurementFrame(header, prefix, gridFactory, () => onRemove(index));
        }
    }

    private void AddMeasurementFrame(string headerText, string bindingPrefix, Func<string, Grid> createGrid, Action removeAction)
    {
        var frame = CreateMeasurementFrame();
        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End
        };
        removeButton.Clicked += (_, __) => removeAction();

        var mainLayout = new StackLayout { HorizontalOptions = LayoutOptions.Fill };
        mainLayout.Add(CreateHeaderGrid(removeButton, headerText, bindingPrefix));
        mainLayout.Add(createGrid(bindingPrefix));
        frame.Content = mainLayout;
        DynamicFormFields.Children.Add(frame);
    }

    private Frame CreateMeasurementFrame()
    {
        return new Frame
        {
            BorderColor = Application.Current?.Resources["Primary"] as Color ?? Colors.Orange,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 10),
            CornerRadius = 10,
            BackgroundColor = Colors.Transparent,
            AutomationId = "MeasurementForm" + (++childFormIdCounter),
            HasShadow = true,
        };
    }

    private static Label CreateLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Start
        };
    }

    private static Grid CreateHeaderGrid(Button button, string headerTxt, string bindingPrefix)
    {
        var headerGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
            },
            RowSpacing = 10
        };

        headerGrid.Add(CreateLabel(headerTxt), 0, 0);
        headerGrid.Add(button, 1, 0);
        headerGrid.AddWithSpan(EntryLabelGrid("টাকার পরিমান : ", $"{bindingPrefix}.Amount"), 1, 0, 1, 2);
        headerGrid.AddWithSpan(EntryLabelGrid("সংখ্যা :", $"{bindingPrefix}.Quantity"), 2, 0, 1, 2);
        return headerGrid;
    }

    private static Grid CreateSelowarGrid(string prefix)
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 10,
            RowSpacing = 10,
        };

        var labels = new[] { "লম্বা", "হিপ", "কমর", "নেছ" };
        var bindings = new[] { $"{prefix}.Length", $"{prefix}.Hip", $"{prefix}.Komor", $"{prefix}.Ness" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
            measurementGrid.Add(entries[i], i % 3, i / 3);
        return measurementGrid;
    }

    private static Grid CreateArabianGrid(string prefix)
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 10,
            RowSpacing = 10,
        };

        var labels = new[] { "লম্বা", "তিরা", "হাতা", "বের", "কফ", "মুহরী", "কমর", "রাকাবা", "নেচ" };
        var bindings = new[] { $"{prefix}.Length", $"{prefix}.Tira", $"{prefix}.Hata", $"{prefix}.Ber", $"{prefix}.Cuff", $"{prefix}.Mohori", $"{prefix}.Komor", $"{prefix}.Rakaba", $"{prefix}.Ness" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
            measurementGrid.Add(entries[i], i % 3, i / 3);
        return measurementGrid;
    }

    private static Grid CreatePanjabiGrid(string prefix)
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 10,
            RowSpacing = 10,
        };

        var labels = new[] { "লম্বা", "সিনা", "কমর", "হাতা", "কফ", "মুহরী", "রাকাবা" };
        var bindings = new[] { $"{prefix}.Length", $"{prefix}.Sina", $"{prefix}.Komor", $"{prefix}.Hata", $"{prefix}.Cuff", $"{prefix}.Mohori", $"{prefix}.Rakaba" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
            measurementGrid.Add(entries[i], i % 3, i / 3);
        return measurementGrid;
    }

    private static VerticalStackLayout EntryWithLabel(string label, string binding)
    {
        var vs = new VerticalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Start };
        vs.Add(CreateLabel(label));
        vs.Add(CreateBorder(binding));
        return vs;
    }

    private static Border CreateBorder(string binding)
    {
        var entry = new Entry { Keyboard = Keyboard.Numeric, MinimumWidthRequest = 140 };
        entry.SetBinding(Entry.TextProperty, binding);
        return new Border { StrokeShape = new RoundRectangle(), Content = entry };
    }

    private static Grid EntryLabelGrid(string label, string binding)
    {
        var entryGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            VerticalOptions = LayoutOptions.Center,
        };
        entryGrid.Add(CreateLabel(label), 0, 0);
        entryGrid.Add(CreateBorder(binding), 1, 0);
        return entryGrid;
    }
}
