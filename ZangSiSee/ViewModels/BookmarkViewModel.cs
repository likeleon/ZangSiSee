using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class BookmarkViewModel : BaseViewModel
    {
        public Bookmark Bookmark { get; }
        public BookInfo BookInfo { get; }

        public BookmarkViewModel(Bookmark bookmark, BookInfo bookInfo)
        {
            Bookmark = bookmark;
            BookInfo = bookInfo;
        }
    }
}
