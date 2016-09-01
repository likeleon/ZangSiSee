using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZangSiSee.Interfaces;
using ZangSiSee.Models;
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
                    LocalRefresh().Forget();
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
            await LocalRefresh();
            if (Comics.Count <= 0)
                await RemoteRefresh();
        }

        async Task RemoteRefresh()
        {
            using (new Busy(this))
            {
                try
                {
                    await ZangSiSiService.Instance.GetAllComics();
                    await LocalRefresh();
                }
                catch (Exception e)
                {
                    "만화 목록을 가져오지 못했습니다.".ToToast(ToastNotificationType.Warning);
                    HandleException(e, false);
                }
            }
        }

        async Task LocalRefresh()
        {
            Comics.Clear();

            var comicsToUpdate = new List<Comic>();
            foreach (var comic in DataManager.Instance.AllComics())
            {
                if (!SearchText.IsNullOrEmpty() && !comic.Title.Contains(SearchText))
                    continue;

                var info = DataManager.Instance.GetBookInfos(comic).FirstOrDefault();
                if (info == null)
                    comicsToUpdate.Add(comic);

                Comics.Add(new ComicViewModel(comic) { Info = info });
            }

            foreach (var comic in comicsToUpdate)
            {
                try
                {
                    var info = await DaumApi.Instance.GetBookInfo(comic, 1) ?? DefaultInfo(comic);
                    DataManager.Instance.InsertOrReplace(info);
                    var vm = Comics.FirstOrDefault(c => c.Comic == comic);
                    if (vm != null)
                        vm.Info = info;
                }
                catch (Exception e)
                {
                    HandleException(e, false);
                }
            }
        }

        BookInfo DefaultInfo(Comic comic) => new BookInfo() { BookTitle = comic.Title, ComicTitle = comic.Title };
    }
}
