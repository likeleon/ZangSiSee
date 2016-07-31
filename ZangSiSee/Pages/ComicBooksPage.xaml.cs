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

            await ViewModel.GetBooks().ConfigureAwait(false);
        }
    }

    public partial class ComicBooksPageXaml : BaseContentPage<ComicBooksViewModel>
    {
    }
}
