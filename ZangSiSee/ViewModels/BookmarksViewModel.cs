using System.Collections.ObjectModel;
using System.Linq;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class BookmarksViewModel : BaseViewModel
    {
        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        public BookmarksViewModel()
        {
            Title = "북마크";
            Bookmarks = new ObservableCollection<BookmarkViewModel>(DataManager.Instance.AllBookmarks().Select(b => new BookmarkViewModel(b)));
        }
    }
}
