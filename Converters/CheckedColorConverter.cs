using System.Globalization;

namespace MYPM.Converters;

public class CheckedColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var target = (bool)value!;
        if (target)
            return Colors.White;
        else
            return Colors.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (string)value!;
    }
}
