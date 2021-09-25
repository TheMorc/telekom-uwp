using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Telekom
{
    public sealed partial class AppShell : Page
    {
        
        public AppShell()
        {
            this.InitializeComponent();

            hamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            hamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();

            frame.Navigate(typeof(Dashboard));
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            frame.Navigate(menuItem.PageType);
        }
        
        
        
    }

    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        public static List<MenuItem> GetMainItems()
        {
            var items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Home, Name = App.resourceLoader.GetString("Overview/Text"), PageType = typeof(Dashboard) },
                new MenuItem() { Icon = Symbol.Page2, Name = App.resourceLoader.GetString("Invoices/Text"), PageType = typeof(Dashboard) },
                new MenuItem() { Icon = Symbol.Contact2, Name = App.resourceLoader.GetString("Profile/Text"), PageType = typeof(Dashboard) }
            };
            return items;
        }

        public static List<MenuItem> GetOptionsItems()
        {
            var items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Setting, Name = App.resourceLoader.GetString("Settings/Text"), PageType = typeof(Dashboard) }
            };
            return items;
        }
    }
}
