using Android.App;
using Android.Content.PM;
using Android.OS;

namespace TwoPaneXAMLDemo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
    | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.UiMode)]
public class MainActivity : MauiAppCompatActivity
{
}
