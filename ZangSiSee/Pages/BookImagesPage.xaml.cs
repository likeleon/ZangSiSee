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

            Title = ViewModel.Book.Title;

            ShowPageWithThrottle = Exts.Throttle<ValueChangedEventArgs>(async (_, e) =>
            {
                await ViewModel.ShowPage(ViewModel.SliderPageNumber);
            }, TimeSpan.FromMilliseconds(500));

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
