using System.Collections.ObjectModel;
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

        public ComicsViewModel()
        {
            Title = "만화 목록";
        }

        public async Task Refresh()
        {
            LocalRefresh();
            if (Comics.Count <= 0)
                await RemoteRefresh();
        }

        async Task RemoteRefresh()
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
            foreach (var comic in DataManager.Instance.AllComics())
            {
                if (!SearchText.IsNullOrEmpty() && !comic.Title.Contains(SearchText))
                    continue;

                Comics.Add(new ComicViewModel() { Comic = comic });
            }
        }
    }
}
