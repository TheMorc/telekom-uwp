using System;
using System.Collections.Generic;
using Telekom.Views;
using Windows.UI.Xaml.Controls;

namespace Telekom
{
    public sealed partial class AppShell : Page
    {

        public AppShell()
        {
            InitializeComponent();

            hamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            hamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();

            frame.Navigate(typeof(Overview));
            commandBarRefresh.Visibility = App.commandBarRefreshVisible;
            hamburgerMenuControl.SelectedIndex = 0;
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem menuItem = e.ClickedItem as MenuItem;
            if (menuItem.PageType != frame.CurrentSourcePageType)
            {
                frame.Navigate(menuItem.PageType);
                commandBarRefresh.Visibility = App.commandBarRefreshVisible;
                commandBarHeader.Text = App.resourceLoader.GetString(frame.CurrentSourcePageType.ToString().Replace("Telekom.Views.", "") + "/Text");
            }
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            frame.Navigate(frame.CurrentSourcePageType);
            commandBarRefresh.Visibility = App.commandBarRefreshVisible;
            commandBarHeader.Text = App.resourceLoader.GetString(frame.CurrentSourcePageType.ToString().Replace("Telekom.Views.", "") + "/Text");
        }

        private void Frame_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            App.frameHeight = Root.ActualHeight;
        }
    }

    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        public static List<MenuItem> GetMainItems()
        {
            List<MenuItem> items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Home, Name = App.resourceLoader.GetString("Overview/Text").Substring(0,1) + App.resourceLoader.GetString("Overview/Text").Substring(1).ToLower(), PageType = typeof(Overview) },
                new MenuItem() { Icon = Symbol.ProtectedDocument, Name = App.resourceLoader.GetString("Invoices/Text").Substring(0,1) + App.resourceLoader.GetString("Invoices/Text").Substring(1).ToLower(), PageType = typeof(Invoices) },
                new MenuItem() { Icon = Symbol.Contact2, Name = App.resourceLoader.GetString("Profile/Text").Substring(0,1) + App.resourceLoader.GetString("Profile/Text").Substring(1).ToLower(), PageType = typeof(Profile) },
                new MenuItem() { Icon = (Symbol)59327, Name = App.resourceLoader.GetString("Eshop/Text").Substring(0,1) + App.resourceLoader.GetString("Eshop/Text").Substring(1).ToLower(), PageType = typeof(Eshop) },
                new MenuItem() { Icon = Symbol.Shop, Name = App.resourceLoader.GetString("Gifts/Text").Substring(0,1) + App.resourceLoader.GetString("Gifts/Text").Substring(1).ToLower(), PageType = typeof(Gifts) },
                new MenuItem() { Icon = (Symbol)61766, Name = App.resourceLoader.GetString("Magenta1/Text").Substring(0,1) + App.resourceLoader.GetString("Magenta1/Text").Substring(1).ToLower(), PageType = typeof(Magenta1) }
            };
            return items;
        }

        public static List<MenuItem> GetOptionsItems()
        {
            List<MenuItem> items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Setting, Name = App.resourceLoader.GetString("Settings/Text").Substring(0,1) + App.resourceLoader.GetString("Settings/Text").Substring(1).ToLower(), PageType = typeof(Settings) }
            };
            return items;
        }
    }
}
