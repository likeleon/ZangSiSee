using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class BookmarkViewModel : BaseViewModel
    {
        public Bookmark Bookmark { get; }

        public BookmarkViewModel(Bookmark bookmark)
        {
            Bookmark = bookmark;
        }
    }
}
