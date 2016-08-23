using Xamarin.Forms;

namespace ZangSiSee.Pages
{
    public class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            BindingContext = new BaseViewModel();
            Master = new ContentPage() { Title = "메뉴" };
            Detail = new NavigationPage(new ComicsPage());
        }
    }
}
