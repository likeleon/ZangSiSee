using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZangSiSee.Interfaces;
using ZangSiSee.Pages;
using ZangSiSee.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ZangSiSee
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DaumApi.Instance.ApiKey = "829a1577c965659dff4f4e7e7b57fd96";
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
