using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class ComicsViewModel : BaseViewModel
    {
        public ObservableCollection<ComicViewModel> Comics { get; } = new ObservableCollection<ComicViewModel>();

        public async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                var task = ZangSiSiService.Instance.GetAllComics();
                await RunSafe(task);

                if (task.IsFaulted)
                    return;

                LocalRefresh();
            }
        }

        void LocalRefresh()
        {
            Comics.Clear();
            foreach (var comic in DataManager.Instance.Comics.Values)
                Comics.Add(new ComicViewModel() { Comic = comic });
        }
    }
}
