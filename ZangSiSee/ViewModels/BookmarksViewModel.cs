using System.Collections.ObjectModel;
using System.Linq;
using ZangSiSee.Models;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class BookmarksViewModel : BaseViewModel
    {
        public ObservableCollection<BookmarkViewModel> Bookmarks { get; } = new ObservableCollection<BookmarkViewModel>();

        public BookmarksViewModel()
        {
            Title = "북마크";
        }

        public void Refresh()
        {
            Bookmarks.Clear();
            foreach (var bookmark in DataManager.Instance.AllBookmarks().OrderByDescending(b => b.CreationTime))
                Bookmarks.Add(new BookmarkViewModel(bookmark, GetBookInfo(bookmark)));
        }

        BookInfo GetBookInfo(Bookmark bookmark)
        {
            var book = DataManager.Instance.GetBook(bookmark.BookTitle);
            if (book == null)
                return null;
            return DataManager.Instance.GetBookInfo(book);
        }
    }
}
