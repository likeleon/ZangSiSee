using System.Collections.ObjectModel;
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
            foreach (var bookmark in DataManager.Instance.AllBookmarks())
                Bookmarks.Add(new BookmarkViewModel(bookmark));
        }
    }
}
