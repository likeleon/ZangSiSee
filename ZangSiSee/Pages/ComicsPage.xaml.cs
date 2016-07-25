using ZangSiSee.ViewModels;

namespace ZangSiSee.Pages
{
    public partial class ComicsPage : ComicsXaml
    {
        public ComicsPage()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            InitializeComponent();
            Title = "만화 리스트";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.RemoteRefresh().ConfigureAwait(false);
        }
    }

    public partial class ComicsXaml : BaseContentPage<ComicsViewModel>
    {
    }
}
