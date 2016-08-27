using System;
using Xamarin.Forms;
using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookImagesPage : BookImagesPageXaml
    {
        EventHandler<ValueChangedEventArgs> ShowPageWithThrottle;
        int _startingPage;

        public BookImagesPage(Book book, int startingPage = 0)
        {
            ViewModel.Book = book;
            _startingPage = startingPage;
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
            await ViewModel.Initialize(_startingPage).ConfigureAwait(false);
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
