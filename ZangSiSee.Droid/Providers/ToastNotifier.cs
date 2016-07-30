using Android.Widget;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZangSiSee.Interfaces;

[assembly: Dependency(typeof(ZangSiSee.Droid.Providers.ToastNotifier))]

namespace ZangSiSee.Droid.Providers
{
    public class ToastNotifier : IToastNotifier
    {
        public Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null)
        {
			var taskCompletionSource = new TaskCompletionSource<bool>();
            Toast.MakeText(Forms.Context, description, ToastLength.Short).Show();
            return taskCompletionSource.Task;
        }

        public void HideAll()
        {
        }
    }
}
