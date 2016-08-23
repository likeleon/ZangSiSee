using System;
using Xamarin.Forms;
using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookImagesPage : BookImagesPageXaml
    {
        EventHandler<ValueChangedEventArgs> ShowPageWithThrottle;

        public BookImagesPage(Book book)
        {
            ViewModel.Book = book;
            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();

            var weakSelf = new WeakReference<BookImagesPage>(this);
            ViewModel.IsFullScreenChanged += (_, fullScreen) =>
            {
                BookImagesPage self;
                if (!weakSelf.TryGetTarget(out self))
                    return;

                NavigationPage.SetHasNavigationBar(this, !fullScreen);
            };

            ShowPageWithThrottle = Exts.Throttle<ValueChangedEventArgs>(async (_, e) =>
            {
                await ViewModel.ShowPage(ViewModel.SliderPageNumber);
            }, TimeSpan.FromMilliseconds(500));

            ViewModel.IsFullScreen = true;
            await ViewModel.GetImages().ConfigureAwait(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageSlider.ValueChanged += ShowPageWithThrottle;
        }

        protected override void OnDisappearing()
        {
            pageSlider.ValueChanged -= ShowPageWithThrottle;
            base.OnDisappearing();
        }
    }

    public partial class BookImagesPageXaml : BaseContentPage<BookImagesViewModel>
    {
    }
}
