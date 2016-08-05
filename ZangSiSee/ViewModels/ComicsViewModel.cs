using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class ComicsViewModel : BaseViewModel
    {
        public ObservableCollection<ComicViewModel> Comics { get; } = new ObservableCollection<ComicViewModel>();

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetPropertyChanged(ref _searchText, value))
                    LocalRefresh();
            }
        }

        public ICommand RefreshComicsCommand => new Command(async () => { await RemoteRefresh(); });

        string _searchText;

        public async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                await ExceptionSafe(ZangSiSiService.Instance.GetAllComics());
                LocalRefresh();
            }
        }

        void LocalRefresh()
        {
            Comics.Clear();
            foreach (var comic in DataManager.Instance.Comics.Values.OrderBy(c => c.Title))
            {
                if (!SearchText.IsNullOrEmpty() && !comic.Title.Contains(SearchText))
                    continue;

                Comics.Add(new ComicViewModel() { Comic = comic });
            }
        }
    }
}
