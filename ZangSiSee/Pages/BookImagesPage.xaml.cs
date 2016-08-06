using Xamarin.Forms;
using ZangSiSee.Interfaces;
using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookImagesPage : BookImagesPageXaml
    {
        public BookImagesPage(Book book)
        {
            ViewModel.Book = book;
            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();
            Title = ViewModel.Book.Title;

            await ViewModel.GetImages().ConfigureAwait(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateFullScreen();

            ViewModel.IsFullScreenChanged += OnIsFullScreenChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UpdateFullScreen();

            ViewModel.IsFullScreenChanged -= OnIsFullScreenChanged;
        }

        void OnIsFullScreenChanged(object sender, bool e)
        {
            UpdateFullScreen();
        }

        void UpdateFullScreen()
        {
            if (ViewModel.IsFullScreen)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                DependencyService.Get<IStatusBar>().Hide();
            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, true);
                DependencyService.Get<IStatusBar>().Show();
            }
        }
    }

    public partial class BookImagesPageXaml : BaseContentPage<BookImagesViewModel>
    {
    }
}
