using System.Collections.ObjectModel;
using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class ComicsViewModel : BaseViewModel
    {
        public ObservableCollection<ComicViewModel> Comics { get; } = new ObservableCollection<ComicViewModel>();

        public ComicsViewModel()
        {
            foreach (var title in new string[] { "은혼", "오늘부터 우리는" })
                Comics.Add(new ComicViewModel() { Comic = new Comic(title) });
        }
    }
}
