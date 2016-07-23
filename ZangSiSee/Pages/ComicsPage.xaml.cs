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
            Title = "Comics";
        }
    }

    public partial class ComicsXaml : BaseContentPage<ComicsViewModel>
    {
    }
}
