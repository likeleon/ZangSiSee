using Xamarin.Forms;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            list.ItemSelected += OnListItemSelected;
        }

        protected override void OnDisappearing()
        {
            list.ItemSelected -= OnListItemSelected;

            base.OnDisappearing();
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (list.SelectedItem == null)
                return;

            var vm = list.SelectedItem as BookViewModel;
            list.SelectedItem = null;

            await Navigation.PushAsync(new BookImagesPage(vm.Book));
        }
    }

    public partial class ComicBooksPageXaml : BaseContentPage<ComicBooksViewModel>
    {
    }
}
