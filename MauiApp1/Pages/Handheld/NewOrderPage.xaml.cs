using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.StyleSheets;

namespace MauiApp1.Pages.Handheld;

public partial class NewOrderPage : ContentPage
{
    private int childFormIdCounter = 0;

    public NewOrderPage()
    {
        InitializeComponent();
    }

    private void OnClothTypeSelected(object sender, EventArgs e)
    {
        Picker picker = (Picker)sender;
        string? selectedClothType = picker.SelectedItem?.ToString();

        if (selectedClothType != null)
        {
            if (selectedClothType == "Shirt")
            {
                AppendShirtForm();
            }
            else if (selectedClothType == "Pant")
            {
                AppendPantForm();
            }
        }
    }

    // Method to append Shirt form fields
    private void AppendShirtForm()
    {
        var shirtForm = new StackLayout
        {
            Padding = new Thickness(0, 10),
            Spacing = 5,
            AutomationId = "ShirtForm" + (++childFormIdCounter)
        };

        // Add form fields to Shirt form
        shirtForm.Children.Add(CreateEntry("Length"));
        shirtForm.Children.Add(CreateEntry("Chest"));
        shirtForm.Children.Add(CreateEntry("Stomach"));
        shirtForm.Children.Add(CreateEntry("Seat"));

        // Add remove button with an 'X' icon
        AddRemoveButton(shirtForm, "Shirt");

        DynamicFormFields.Children.Add(shirtForm);
    }

    // Method to append Pant form fields
    private void AppendPantForm()
    {
        var pantForm = new StackLayout
        {
            Padding = new Thickness(0, 10),
            Spacing = 5,
            AutomationId = "PantForm" + (++childFormIdCounter)
        };

        pantForm.Children.Add(CreateEntry("Length"));
        pantForm.Children.Add(CreateEntry("Stomach"));
        pantForm.Children.Add(CreateEntry("Seat"));

        AddRemoveButton(pantForm, "Pant");
        DynamicFormFields.Children.Add(pantForm);
    }

    private Entry CreateEntry(string placeholder)
    {
        return new Entry
        {
            Placeholder = placeholder,
            Margin = new Thickness(0, 5, 0, 5)
        };
    }

    private void AddRemoveButton(StackLayout formLayout, string formType)
    {
        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            WidthRequest = 30,
            HeightRequest = 30,
            CornerRadius = 15,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        };

        removeButton.Clicked += (s, e) =>
        {
            DynamicFormFields.Children.Remove(formLayout);
        };
        formLayout.Children.Add(removeButton);
    }

    // Event handler for adding a new measurement type
    private void OnAddMeasurementType(object sender, EventArgs e)
    {
        ////var measurementFrame = new Frame
        ////{
        ////    AutomationId = "MeasurementForm" + (++childFormIdCounter),
        ////    BorderColor = Colors.Orange,
        ////    CornerRadius = 10,
        ////    Padding = 10,
        ////    Margin = new Thickness(0, 10),
        ////    BackgroundColor = Colors.Transparent,
        ////    Content = new StackLayout
        ////    {
        ////        Children =
        ////            {
        ////                CreateMeasurementHeader(),
        ////                CreateMeasurementFields()
        ////            }
        ////    }
        ////};

        ////DynamicFormFields.Children.Add(measurementFrame);

        genareteCode();
    }

    private StackLayout CreateMeasurementHeader()
    {
        var headerLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Fill
        };

        var label = new Label
        {
            Text = "Measurement",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Start
        };

        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            HorizontalOptions = LayoutOptions.End
        };

        removeButton.Clicked += (s, e) =>
        {
            var buttonParent = (StackLayout)((Button)s).Parent;
            var formFrame = (Frame)buttonParent.Parent;
            DynamicFormFields.Children.Remove(formFrame);
        };

        headerLayout.Children.Add(label);
        headerLayout.Children.Add(removeButton);

        return headerLayout;
    }

    private Grid CreateMeasurementFields()
    {
        var grid = new Grid
        {
            ColumnDefinitions =
                [
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                ],
            RowDefinitions =
                [
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                ],
            ColumnSpacing = 10,
            RowSpacing = 10
        };

        // Add measurement labels and entries
        grid.Add(new Label { Text = "Length" }, 0, 0);
        grid.Add(new Entry { Placeholder = "Enter Length" }, 1, 0);

        grid.Add(new Label { Text = "Chest" }, 0, 1);
        grid.Add(new Entry { Placeholder = "Enter Chest" }, 1, 1);

        grid.Add(new Label { Text = "Stomach" }, 0, 2);
        grid.Add(new Entry { Placeholder = "Enter Stomach" }, 1, 2);

        return grid;
    }
    private void genareteCode()
    {
        var frame = new Frame
        {
            BorderColor = Application.Current?.Resources["Primary"] as Color ?? Colors.Orange,
            Padding = new Thickness(10),
            Margin = new Thickness(0, 10),
            CornerRadius = 10,
            BackgroundColor = Colors.Transparent,
            AutomationId = "MeasurementForm" + (++childFormIdCounter),
        };

        var mainLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill
        };

        var headerGrid = new Grid
        {
            ColumnDefinitions =
            [
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            ]
        };

        var measurementLabel = new Label
        {
            Text = "Measurement",
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Start
        };

        var removeButton = new Button
        {
            Text = "X",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End
        };

        headerGrid.Add(measurementLabel, 0, 0);
        headerGrid.Add(removeButton, 1, 0);

        // Create the measurement fields grid
        var measurementGrid = new Grid
        {
            ColumnDefinitions =
            [
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            ],
            RowDefinitions =
            [
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            ],
            ColumnSpacing = 10,
            RowSpacing = 10
        };

        // Add Length label and entry
        var lengthLabel = new Label { Text = "Length"};
        var lengthEntry = new Entry();
        var lengthBorder = new Border
        {
            StrokeShape = new RoundRectangle(),
            Content = lengthEntry
        };

        measurementGrid.Add(lengthLabel, 0, 0);
        measurementGrid.Add(lengthBorder, 1, 0);

        // Add Chest label and entry
        var chestLabel = new Label { Text = "Chest" };
        var chestEntry = new Entry();
        var chestBorder = new Border
        {
            StrokeShape = new RoundRectangle(),
            Content = chestEntry
        };

        measurementGrid.Add(chestLabel, 0, 1);
        measurementGrid.Add(chestBorder, 1, 1);

        // Add Stomach label and entry
        var stomachLabel = new Label { Text = "Stomach"};
        var stomachEntry = new Entry();
        var stomachBorder = new Border
        {
            StrokeShape = new RoundRectangle(),
            Content = stomachEntry
        };

        measurementGrid.Add(stomachLabel, 0, 2);
        measurementGrid.Add(stomachBorder, 1, 2);

        // Add the header grid and measurement grid to the main layout
        mainLayout.Add(headerGrid);
        mainLayout.Add(measurementGrid);

        // Set the frame content
        frame.Content = mainLayout;

        DynamicFormFields.Children.Add(frame);

        // Event handler for remove button
        removeButton.Clicked += (s, e) =>
        {
            DynamicFormFields.Children.Remove(frame);
        };
    }
}
