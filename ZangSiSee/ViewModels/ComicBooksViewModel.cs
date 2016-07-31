using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZangSiSee.Models;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class ComicBooksViewModel : BaseViewModel
    {
        public Comic Comic { get; set; }
        public ObservableCollection<BookViewModel> Books { get; } = new ObservableCollection<BookViewModel>();

        public ICommand RefreshBooksCommand => new Command(async () => { await RemoteRefresh(); });

        public async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                var books = await ZangSiSiService.Instance.GetBooks(Comic);

                Books.Clear();
                foreach (var book in books.OrderBy(b => b.Order))
                    Books.Add(new BookViewModel() { Book = book });
            }
        }
    }
}
