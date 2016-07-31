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

        public ICommand RefreshComicsCommand => new Command(async () => { await RemoteRefresh(); });

        public async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                var comics = await ZangSiSiService.Instance.GetAllComics();

                Comics.Clear();
                foreach (var comic in comics.OrderBy(c => c.Title))
                    Comics.Add(new ComicViewModel() { Comic = comic });
            }
        }
    }
}
