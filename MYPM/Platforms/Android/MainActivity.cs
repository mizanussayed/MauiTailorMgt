using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MYPM.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set status bar color globally
        SetStatusBarColor();
    }

    private void SetStatusBarColor()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Window != null)
        {
            if (MauiApp.Current?.Resources.TryGetValue("SurfaceVariant", out var surfaceVariant) == true
                && surfaceVariant is Color color)
            {
                Window.SetStatusBarColor(color.ToPlatform());

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var windowInsetsController = WindowCompat.GetInsetsController(Window, Window.DecorView);
                    windowInsetsController.AppearanceLightStatusBars = true;
                }
            }
        }
    }
}
