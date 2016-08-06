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

            EnterOrExitFullScreen(ViewModel.IsFullScreen);
            ViewModel.IsFullScreenChanged += OnIsFullScreenChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.IsFullScreenChanged -= OnIsFullScreenChanged;
            ExitFullScreen();
        }

        void OnIsFullScreenChanged(object sender, bool e) => EnterOrExitFullScreen(e);

        void EnterOrExitFullScreen(bool enter)
        {
            if (enter)
                EnterFullScreen();
            else
                ExitFullScreen();
        }

        void EnterFullScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            DependencyService.Get<IStatusBar>().Hide();
        }

        void ExitFullScreen()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            DependencyService.Get<IStatusBar>().Show();
        }
    }

    public partial class BookImagesPageXaml : BaseContentPage<BookImagesViewModel>
    {
    }
}
