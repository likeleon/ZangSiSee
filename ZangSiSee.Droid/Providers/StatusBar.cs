using Android.App;
using Android.Views;
using Xamarin.Forms;
using ZangSiSee.Interfaces;

[assembly: Dependency(typeof(ZangSiSee.Droid.Providers.StatusBar))]
namespace ZangSiSee.Droid.Providers
{
    public class StatusBar : IStatusBar
    {
        public void Hide()
        {
            var activity = (Activity)Forms.Context;
            activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        public void Show()
        {
            var activity = (Activity)Forms.Context;
            activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }
    }
}