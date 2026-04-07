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
            var items = Basket.currentBasket.ProductsInBasket.GroupBy(p => p.ID).Select(g => new
            {
                Product = g.First(),
                Quantity = g.Count(),
                Total = Basket.GetPriceWithDiscount(g.First()) * g.Count()
            })
                .ToList();

            basketItems.ItemsSource = null;
            basketItems.ItemsSource = items;
        }

        private void UpdateTotalPrice()
        {
            double total = Basket.currentBasket.ProductsInBasket.Sum(p => Basket.GetPriceWithDiscount(p));
            totalPriceTB.Text = $"{total}₽";
        }

        private void increase_Click(object sender, RoutedEventArgs e)
        {
            dynamic item = (sender as Button).DataContext;

            Basket.currentBasket.AddProduct(item.Product);

            LoadBasketItems();
            UpdateTotalPrice();
        }

        private void decrease_Click(object sender, RoutedEventArgs e)
        {
            dynamic item = (sender as Button).DataContext;

            Basket.currentBasket.RemoveProduct(item.Product);

            LoadBasketItems();
            UpdateTotalPrice();
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            dynamic item = (sender as Button).DataContext;

            Basket.currentBasket.ProductsInBasket
                .RemoveAll(p => p.ID == item.Product.ID);

            LoadBasketItems();
            UpdateTotalPrice();
        }

        private void backToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new productPage());
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            if (!Basket.currentBasket.ProductsInBasket.Any())
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            new orderWindow().ShowDialog();

            LoadBasketItems();
            UpdateTotalPrice();
        }
    }
}