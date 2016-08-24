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

        public async Task Refresh()
        {
            LocalRefresh();
            if (Books.Count <= 0)
                await RemoteRefresh();
        }

        async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                await ExceptionSafe(ZangSiSiService.Instance.GetBooks(Comic));
                LocalRefresh();
            }
        }

        void LocalRefresh()
        {
            Books.Clear();
            foreach (var book in DataManager.Instance.GetBooks(Comic).OrderBy(b => b.Order))
                Books.Add(new BookViewModel(book));
        }
    }
}
