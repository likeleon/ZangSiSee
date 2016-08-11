using ZangSiSee.Models;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class BookImagesPage : BookImagesPageXaml
    {
        public BookImagesPage(Book book)
        {
            ViewModel.Book = book;
            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();
            Title = ViewModel.Book.Title;

            await ViewModel.GetImages().ConfigureAwait(false);
        }
    }

    public partial class BookImagesPageXaml : BaseContentPage<BookImagesViewModel>
    {
    }
}
