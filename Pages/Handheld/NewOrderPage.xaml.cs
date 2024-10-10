using Microsoft.Maui.Controls.Shapes;

namespace MYPM.Pages.Handheld;

public partial class NewOrderPage : ContentPage
{
    private int childFormIdCounter = 0;

    public NewOrderPage()
    {
        InitializeComponent();
        BindingContext = new NewOrderPageViewModel();
    }



    private void OnAddMeasurementType(object sender, EventArgs e)
    {
        var orderFor = (BindingContext as NewOrderPageViewModel)?.OrderFor;
        var frame = CreateMeasurementFrame();
        frame.Content = CreateMainLayout(frame, orderFor);
        if (frame.Content is not null)
        {
            DynamicFormFields.Children.Add(frame);
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

    private StackLayout CreateMainLayout(Frame frame, string? orderFor)
    {
        var mainLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill
        };
        var removeButton = CreateRemoveButton(frame);
        if (orderFor is not null)
        {
            switch (orderFor.ToLower())
            {
                case "arabian":
                    mainLayout.Add(CreateHeaderGrid(removeButton, "এরাবিয়ান এর"));
                    mainLayout.Add(CreateArabianGrid());
                    break;
                case "panjabi":
                    mainLayout.Add(CreateHeaderGrid(removeButton, "পাঞ্জাবির"));
                    mainLayout.Add(CreatePanjabiGrid());
                    break;
                default:
                    mainLayout.Add(CreateHeaderGrid(removeButton, "সেলোয়ার এর"));
                    mainLayout.Add(CreateSelowarGrid());
                    break;
            }
        }
        return mainLayout;
    }

    private Button CreateRemoveButton(Frame frame)
    {
        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End
        };

        removeButton.Clicked += (s, e) => DynamicFormFields.Children.Remove(frame);
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

    private static Grid CreateHeaderGrid(Button button, string headerTxt)
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
        headerGrid.AddWithSpan(EntryLabelGrid("টাকার পরিমান :"), 1, 0, 1, 2);
        headerGrid.AddWithSpan(EntryLabelGrid($"{headerTxt} সংখ্যা :"), 2, 0, 1, 2);

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
        var entries = labels.Select(EntryWithLabel).ToArray();

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
        var entries = labels.Select(EntryWithLabel).ToArray();

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
        var entries = labels.Select(EntryWithLabel).ToArray();

        for (int i = 0; i < entries.Length; i++)
        {
            measurementGrid.Add(entries[i], i % 2, i / 2);
        }

        return measurementGrid;
    }

    private static VerticalStackLayout EntryWithLabel(string label)
    {
        var vs = new VerticalStackLayout
        {
            CreateLabel(label),
            CreateBorder()
        };
        vs.Spacing = 10;
        vs.VerticalOptions = LayoutOptions.Start;
        return vs;
    }

    private static Border CreateBorder()
    {
        return new Border
        {
            StrokeShape = new RoundRectangle(),
            Content = new Entry
            {
                Keyboard = Keyboard.Numeric,
                MinimumWidthRequest = 140
            }
        };
    }

    private static Grid EntryLabelGrid(string label)
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
        entryGrid.Add(CreateBorder(), 1, 0);
        return entryGrid;
    }
}
