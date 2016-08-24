using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        public Book Book { get; } 

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
