using Android.OS;
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
using MauiWindow = Microsoft.Maui.Controls.Window;

namespace MYPM.Platforms.Android;

public static class StatusBarHelper
{
    public static void SetStatusBarColor(this MauiWindow window, Microsoft.Maui.Graphics.Color color, bool darkContent = false)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && window?.Handler?.PlatformView is not null)
        {
            var platformWindow = window.Handler.PlatformView as global::Android.App.Activity;
            if (platformWindow?.Window != null)
            {
                platformWindow.Window.SetStatusBarColor(color.ToPlatform());

                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var windowInsetsController = WindowCompat.GetInsetsController(platformWindow.Window, platformWindow.Window.DecorView);
                    windowInsetsController.AppearanceLightStatusBars = darkContent;
                }
            }
        }
    }
}
