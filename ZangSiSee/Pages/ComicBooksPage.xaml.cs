using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class ComicBooksPage : ComicBooksPageXaml
    {
        public ComicBooksPage(Comic comic)
        {
            ViewModel.Comic = comic;

            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();
            Title = ViewModel.Comic.Title;

            await ViewModel.RemoteRefresh().ConfigureAwait(false);
        }
    }

    public partial class ComicBooksPageXaml : BaseContentPage<ComicBooksViewModel>
    {
    }
}
