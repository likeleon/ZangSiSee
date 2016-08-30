using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        public Book Book { get; } 
        public BookInfo Info
        {
            get { return _info; }
            set { SetPropertyChanged(ref _info, value); }
        }

        BookInfo _info;

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
