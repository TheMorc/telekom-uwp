using System;
using System.Collections.Generic;
using Telekom.Views;
using Windows.UI.Xaml.Controls;

namespace Telekom
{
    public sealed partial class AppShell : Page
    {

        private string lastcBText = "";

        public AppShell()
        {
            InitializeComponent();

            hamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            hamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();

            App.commandBarText = App.resourceLoader.GetString("Overview/Text").ToUpper();
            commandBarHeader.Text = App.commandBarText;
            frame.Navigate(typeof(Overview));
            commandBarRefresh.Visibility = App.commandBarRefreshVisible;
            hamburgerMenuControl.SelectedIndex = 0;
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem menuItem = e.ClickedItem as MenuItem;
            if (menuItem.Name.ToUpper() != App.commandBarText)
            {
                App.commandBarText = menuItem.Name.ToUpper();
                commandBarHeader.Text = App.commandBarText;
                frame.Navigate(menuItem.PageType);
                commandBarRefresh.Visibility = App.commandBarRefreshVisible;
            }
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            frame.Navigate(frame.CurrentSourcePageType);
        }

        private void Frame_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            App.frameHeight = Root.ActualHeight - 50;
        }

        private void Frame_LayoutUpdated(object sender, object e)
        {
            if (lastcBText != App.commandBarText)
            {
                commandBarHeader.Text = App.commandBarText;
            }
            lastcBText = App.commandBarText;
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
                new MenuItem() { Icon = Symbol.Home, Name = App.resourceLoader.GetString("Overview/Text"), PageType = typeof(Overview) },
                new MenuItem() { Icon = Symbol.ProtectedDocument, Name = App.resourceLoader.GetString("Invoices/Text"), PageType = typeof(Invoices) },
                new MenuItem() { Icon = Symbol.Contact2, Name = App.resourceLoader.GetString("Profile/Text"), PageType = typeof(Profile) },
                new MenuItem() { Icon = (Symbol)59327, Name = App.resourceLoader.GetString("Eshop/Text"), PageType = typeof(Views.WebView) },
                new MenuItem() { Icon = Symbol.Shop, Name = App.resourceLoader.GetString("Gifts/Text"), PageType = typeof(Views.WebView) },
                new MenuItem() { Icon = (Symbol)61766, Name = App.resourceLoader.GetString("Magenta1/Text"), PageType = typeof(Views.WebView) }
            };
            return items;
        }

        public static List<MenuItem> GetOptionsItems()
        {
            List<MenuItem> items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Setting, Name = App.resourceLoader.GetString("Settings/Text"), PageType = typeof(Settings) }
            };
            return items;
        }
    }
}
