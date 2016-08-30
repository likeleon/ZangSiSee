using System;
using System.Collections.Generic;
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
            await LocalRefresh();
            if (Books.Count <= 0)
                await RemoteRefresh().ConfigureAwait(false);
        }

        async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                await ExceptionSafe(ZangSiSiService.Instance.GetBooks(Comic));
                await LocalRefresh().ConfigureAwait(false);
            }
        }

        async Task LocalRefresh()
        {
            Books.Clear();

            var booksToUpdate = new List<Book>();
            foreach (var book in DataManager.Instance.GetBooks(Comic).OrderBy(b => b.Order))
            {
                var info = DataManager.Instance.GetBookInfo(book);
                if (info == null)
                    booksToUpdate.Add(book);

                Books.Add(new BookViewModel(book) { Info = info });
            }

            foreach (var book in booksToUpdate)
            {
                try
                {
                    var info = await DaumApi.Instance.GetBookInfo(book) ?? DefaultInfo(book);
                    DataManager.Instance.InsertOrReplace(info);
                    var vm = Books.FirstOrDefault(b => b.Book == book);
                    if (vm != null)
                        vm.Info = info;
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            }
        }

        BookInfo DefaultInfo(Book book) => new BookInfo() { BookTitle = book.Title, ComicTitle = book.ComicTitle };
    }
}
