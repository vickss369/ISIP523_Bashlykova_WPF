using PR12;
using PR12.Classes;
using PR12.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для basketPage.xaml
    /// </summary>
    public partial class basketPage : Page
    {
        public basketPage()
        {
            InitializeComponent();

            LoadBasketItems();
            UpdateTotalPrice();
        }

        private void LoadBasketItems()
        {
            basketItems.ItemsSource = null;
            basketItems.ItemsSource = Basket.currentBasket.ProductsInBasket;
        }
            
        private void UpdateTotalPrice()
        {
            totalPriceTB.Text = $"{Basket.currentBasket.TotalPrice}₽";
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;

            if (product != null)
            {
                Basket.currentBasket.RemoveProduct(product);
                LoadBasketItems();
                UpdateTotalPrice();
            }
        }

        private void backToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new productPage());
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            new orderWindow().ShowDialog();

            LoadBasketItems();
            UpdateTotalPrice();
        }
    }
}