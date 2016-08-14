using System;
using Xamarin.Forms;
using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookImagesPage : BookImagesPageXaml
    {
        EventHandler<ValueChangedEventArgs> ShowSlidingPageWithThrottling;

        public BookImagesPage(Book book)
        {
            ViewModel.Book = book;
            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();
            Title = ViewModel.Book.Title;

            ShowSlidingPageWithThrottling = Exts.Throttle<ValueChangedEventArgs>(async (_, e) =>
            {
                await ViewModel.ShowPage(ViewModel.SlidingPageNumber);
            }, TimeSpan.FromMilliseconds(500));

            await ViewModel.GetImages().ConfigureAwait(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageSlider.ValueChanged += UpdateSlidingPageNumber;
            pageSlider.ValueChanged += ShowSlidingPageWithThrottling;
        }

        protected override void OnDisappearing()
        {
            pageSlider.ValueChanged -= ShowSlidingPageWithThrottling;
            pageSlider.ValueChanged -= UpdateSlidingPageNumber;
            base.OnDisappearing();
        }

        void UpdateSlidingPageNumber(object sender, ValueChangedEventArgs e) => ViewModel.SlidingPageNumber = (int)Math.Round(e.NewValue);
    }

    public partial class BookImagesPageXaml : BaseContentPage<BookImagesViewModel>
    {
    }
}
