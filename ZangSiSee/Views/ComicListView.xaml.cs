using Xamarin.Forms;

namespace ZangSiSee.Views
{
    public partial class ComicListView : ListView
    {
        public ComicListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            InitializeComponent();
        }
    }
}
