using Xamarin.Forms;
using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class ComicsPage : ComicsXaml
    {
        public ComicsPage()
        {
            Initialize();
        }

        protected async override void Initialize()
        {
            InitializeComponent();

            await ViewModel.Refresh().ConfigureAwait(false);
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

            var vm = list.SelectedItem as ComicViewModel;
            list.SelectedItem = null;

            await Navigation.PushAsync(new ComicBooksPage(vm.Comic));
        }
    }

    public partial class ComicsXaml : BaseContentPage<ComicsViewModel>
    {
    }
}
