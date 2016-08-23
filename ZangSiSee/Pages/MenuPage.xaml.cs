using System.Linq;

using Xamarin.Forms;

namespace ZangSiSee.Pages
{
    public partial class MenuPage : ContentPage
    {
        readonly RootPage _rootPage;

        readonly HomeMenuItem[] _menuItems = new HomeMenuItem[]
        {
            new HomeMenuItem { Title = "만화 목록", MenuType = MenuType.Comics, Icon = "info.png" },
            new HomeMenuItem { Title = "북마크", MenuType = MenuType.Bookmarks, Icon = "info.png" },
            new HomeMenuItem { Title = "설정", MenuType = MenuType.Settings, Icon = "info.png" },
            new HomeMenuItem { Title = "정보", MenuType = MenuType.About, Icon = "info.png" }
        };

        public MenuPage(RootPage rootPage, MenuType initialMenu)
        {
            InitializeComponent();

            _rootPage = rootPage;

            BindingContext = new BaseViewModel() { Title = "장시시" };

            listViewMenu.ItemsSource = _menuItems;
            listViewMenu.ItemSelected += (_, e) =>
            {
                var item = listViewMenu.SelectedItem as HomeMenuItem;
                if (item == null)
                    return;

                _rootPage.Navigate(item.MenuType);
            };
            listViewMenu.SelectedItem = _menuItems.First(m => m.MenuType == initialMenu);
        }
    }

    public class HomeMenuItem
    {
        public MenuType MenuType { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
