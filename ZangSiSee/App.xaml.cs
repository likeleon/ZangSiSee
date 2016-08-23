using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZangSiSee.Interfaces;
using ZangSiSee.Pages;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ZangSiSee
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<BaseViewModel, Exception>(this, "ExceptionOccured", OnAppExceptionOccured);
            MainPage = new RootPage();
        }

        void OnAppExceptionOccured(BaseViewModel viewModel, Exception exception)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var msg = exception.Message;
                    if (msg.Length > 300)
                        msg = msg.Substring(0, 300);

                    msg.ToToast(ToastNotificationType.Error, "이런...");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
