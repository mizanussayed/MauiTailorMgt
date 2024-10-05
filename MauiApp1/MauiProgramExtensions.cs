using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace MauiApp1;
public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("opensans_semibold.ttf", "OpenSansSemiBold");
            fonts.AddFont("fa_solid.ttf", "FontAwesome");
            fonts.AddFont("fabmdl2.ttf", "Fabric");
        }).UseMauiCommunityToolkit();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder;
    }
}