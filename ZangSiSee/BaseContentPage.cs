using System;
using Xamarin.Forms;

namespace ZangSiSee
{
    public class BaseContentPage<T> : MainBaseContentPage where T : BaseViewModel, new()
    {
        public T ViewModel => viewModel.Value;
        
        readonly Lazy<T> viewModel = Exts.Lazy(() => new T());

        public BaseContentPage()
        {
            BindingContext = ViewModel;
        }
    }

    public class MainBaseContentPage : ContentPage
    {
        protected virtual void Initialize()
        {
        }
    }
}