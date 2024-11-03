using Microsoft.Maui.Controls.Shapes;
using MYPM.Pages.Views;
using MYPM.ViewModels;
using System.Web;

namespace MYPM.Pages.Handheld;

[QueryProperty("NextId", "NextId")]
public partial class NewOrderPage : ContentPage
{
    private int childFormIdCounter = 0;
    public string NextId { get; set; } = string.Empty;
    public NewOrderPage()
   {
        InitializeComponent();
    }

    private void OnAddMeasurementType(object sender, EventArgs e)
    {
        OrderSerialText.Text = "Sn : " + NextId;
        var context = (BindingContext as NewOrderPageViewModel);
        var orderFor = context?.Order.OrderFor?.ToLower();
        var orderText = string.Empty;
        int orderType = 0;
        if (orderFor is null) return;

        string headerText = string.Empty;
        Func<Grid>? createGrid = null;

        switch (orderFor)
        {
            case "arabian":
                if (context?.ArabianOrder == null)
                {
                    context.ArabianOrder = new ArabianOrder();
                    headerText = "এরাবিয়ান এর";
                    createGrid = CreateArabianGrid;
                    orderText = "ArabianOrder";
                    orderType = 1;
                }
                break;
            case "panjabi":
                if (context?.PanjabiOrder == null)
                {
                    context.PanjabiOrder = new PanjabiOrder();
                    headerText = "পাঞ্জাবির";
                    createGrid = CreatePanjabiGrid;
                    orderText = "PanjabiOrder";
                    orderType = 2;
                }
                break;
            case "selowar":
                if (context?.SelowerOrder == null)
                {
                    context.SelowerOrder = new SelowerOrder();
                    headerText = "সেলোয়ার এর";
                    createGrid = CreateSelowarGrid;
                    orderText = "SelowerOrder";
                    orderType = 3;
                }
                break;
            default:
                return;
        }

        if (createGrid is not null)
        {
            var frame = CreateMeasurementFrame();
            var mainLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill
            };

            var removeButton = CreateRemoveButton(frame, context, orderType);
            mainLayout.Add(CreateHeaderGrid(removeButton, headerText, orderText));
            mainLayout.Add(createGrid());
            frame.Content = mainLayout;
            if (frame.Content is not null)
            {
                DynamicFormFields.Children.Add(frame);
            }
        }
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

    private Button CreateRemoveButton(Frame frame, NewOrderPageViewModel binding, int orderType)
    {
        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End
        };



        removeButton.Clicked += (s, e) =>
        {
            DynamicFormFields.Children.Remove(frame);
            if (orderType == 1)
            {
                binding.ArabianOrder = null;
            }
            else if (orderType == 2)
            {
                binding.PanjabiOrder = null;
            }
            else if (orderType == 3)
            {
                binding.SelowerOrder = null;
            }
        };
        return removeButton;
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

    private static Grid CreateHeaderGrid(Button button, string headerTxt, string orderFor)
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

        headerGrid.Add(CreateLabel($"{headerTxt} মাপ"), 0, 0);
        headerGrid.Add(button, 1, 0);
        headerGrid.AddWithSpan(EntryLabelGrid("টাকার পরিমান : ", $"{orderFor}.Amount"), 1, 0, 1, 2);
        headerGrid.AddWithSpan(EntryLabelGrid($"{headerTxt} সংখ্যা :", $"{orderFor}.Quantity"), 2, 0, 1, 2);

        return headerGrid;
    }
    private static Grid CreateSelowarGrid()
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 10,
            RowSpacing = 10,
            Padding = 10
        };

        var labels = new[] { "লম্বা", "হিপ", "কমর", "নেছ" };
        var bindings = new[] { "SelowerOrder.Length", "SelowerOrder.Hip", "SelowerOrder.Komor", "SelowerOrder.Ness" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
        {
            measurementGrid.Add(entries[i], i % 2, i / 2);
        }
        return measurementGrid;
    }

    private static Grid CreateArabianGrid()
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
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
            Padding = 10
        };


        var labels = new[] { "লম্বা", "তিরা", "হাতা", "কফ", "মুহরী", "রাকাবা", "নেছ" };
        var bindings = new[] { "ArabianOrder.Length", "ArabianOrder.Tira", "ArabianOrder.Hata", "ArabianOrder.Cuff", "ArabianOrder.Mohori", "ArabianOrder.Rakaba", "ArabianOrder.Ness" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
        {
            measurementGrid.Add(entries[i], i % 2, i / 2);
        }
        return measurementGrid;
    }

    private static Grid CreatePanjabiGrid()
    {
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 10,
            RowSpacing = 10,
            Padding = 10
        };


        var labels = new[] { "লম্বা", "সিনা", "কমর", "হাতা", "কফ", "মুহরী", "রাকাবা" };
        var bindings = new[] { "PanjabiOrder.Length", "PanjabiOrder.Sina", "PanjabiOrder.Komor", "PanjabiOrder.Hata", "PanjabiOrder.Cuff", "PanjabiOrder.Mohori", "PanjabiOrder.Rakaba" };
        var entries = labels.Zip(bindings, EntryWithLabel).ToArray();
        for (int i = 0; i < entries.Length; i++)
        {
            measurementGrid.Add(entries[i], i % 2, i / 2);
        }
        return measurementGrid;
    }

    private static VerticalStackLayout EntryWithLabel(string label, string binding)
    {
        var vs = new VerticalStackLayout
        {
            CreateLabel(label),
            CreateBorder(binding)
        };
        vs.Spacing = 10;
        vs.VerticalOptions = LayoutOptions.Start;
        return vs;
    }

    private static Border CreateBorder(string binding)
    {
        var entry = new Entry
        {
            Keyboard = Keyboard.Numeric,
            MinimumWidthRequest = 140,
        };
        entry.SetBinding(Entry.TextProperty, binding);
        return new Border
        {
            StrokeShape = new RoundRectangle(),
            Content = entry,
        };
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

    private async void BtnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is not NewOrderPageViewModel context) return;

        context.Order.TotalAmount =
            (context.ArabianOrder?.Amount ?? 0) * (context.ArabianOrder?.Quantity ?? 0) +
            (context.SelowerOrder?.Amount ?? 0) * (context.SelowerOrder?.Quantity ?? 0) +
            (context.PanjabiOrder?.Amount ?? 0) * (context.PanjabiOrder?.Quantity ?? 0);

        if (context.Order.TotalAmount > 0)
        {
            context.Order.DueAmount = context.Order.TotalAmount - context.Order.PaidAmount;
            context.Order.Id = NextId;
            await Shell.Current.Navigation.PushModalAsync(new AddAdvanceAmount(context), true);
        }
        else
        {
            Shell.Current.DisplayAlert("Add Measurement", "Arabian / Panjabi / Selower", "OK");
#if ANDROID
          //  Snackbar.Make("Please Select Any Order Measurement", null, "Ok", TimeSpan.FromSeconds(3));
#elif WINDOWS
           // Shell.Current.DisplayActionSheet("Add Measurement", "Ok", "", FlowDirection.LeftToRight);
#endif

        }
    }
}
