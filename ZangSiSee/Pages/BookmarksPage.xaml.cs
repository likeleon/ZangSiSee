using Xamarin.Forms;
using ZangSiSee.Services;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookmarksPage : BookmarksXaml
    {
        public BookmarksPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            list.ItemSelected += OnListItemSelected;
            ViewModel.Refresh();
        }

        protected override void OnDisappearing()
        {
            list.ItemSelected -= OnListItemSelected;

            base.OnDisappearing();
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (list.SelectedItem == null)
                return;

            var bookmark = (list.SelectedItem as BookmarkViewModel)?.Bookmark;
            if (bookmark == null)
                return;

            list.SelectedItem = null;

            var book = DataManager.Instance.GetBook(bookmark.BookTitle);
            if (book == null)
            {
                await DisplayAlert("북마크", "\"{0}\" 책을 찾을 수 없습니다".F(bookmark.BookTitle), "확인");
                return;
            }
            await Navigation.PushAsync(new BookImagesPage(book, bookmark.PageNumber));
        }
    }

    public partial class BookmarksXaml : BaseContentPage<BookmarksViewModel>
    {
    }
}
