using System.Collections.Generic;
using Xamarin.Forms;

namespace ZangSiSee.Pages
{
    public class RootPage : MasterDetailPage
    {
        static MenuType DefaultMenuType { get; } = MenuType.Comics;

        readonly Dictionary<MenuType, NavigationPage> _pages = new Dictionary<MenuType, NavigationPage>();

        public RootPage()
        {
            BindingContext = new BaseViewModel();
            Master = new MenuPage(this, DefaultMenuType);
            Navigate(DefaultMenuType);
        }

        public void Navigate(MenuType menu)
        {
            NavigationPage page;
            if (!_pages.TryGetValue(menu, out page))
            {
                switch (menu)
                {
                    case MenuType.Comics:
                        page = new NavigationPage(new ComicsPage());
                        _pages.Add(menu, page);
                        break;

                    default:
                        page = new NavigationPage(new ContentPage() { Title = menu.ToString() });
                        _pages.Add(menu, page);
                        break;
                }
            }

            Detail = page;
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
