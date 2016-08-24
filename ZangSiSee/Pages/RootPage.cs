using Xamarin.Forms;
using ZangSiSee.Primitives;

namespace ZangSiSee.Pages
{
    public class RootPage : MasterDetailPage
    {
        static MenuType DefaultMenuType { get; } = MenuType.Comics;

        readonly Cache<MenuType, NavigationPage> _pages = new Cache<MenuType, NavigationPage>(CreatePage);

        public RootPage()
        {
            BindingContext = new BaseViewModel();
            Master = new MenuPage(this, DefaultMenuType);
            Navigate(DefaultMenuType);
        }

        static NavigationPage CreatePage(MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.Comics:
                    return new NavigationPage(new ComicsPage());

                default:
                    return new NavigationPage(new ContentPage() { Title = menuType.ToString() });
            }
        }

        public void Navigate(MenuType menu)
        {
            Detail = _pages[menu];
            IsPresented = false;
        }
    }

    public enum MenuType
    {
        Comics,
        Bookmarks,
        Settings,
        About
    }
}
