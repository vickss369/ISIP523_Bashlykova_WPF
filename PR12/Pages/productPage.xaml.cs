using PR12.Classes;
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
    /// Логика взаимодействия для productPage.xaml
    /// </summary>
    public partial class productPage : Page
    {
        private List<Product> allProducts;
        public static Product selectedProduct;
        private Basket userBasket = new Basket();

        public productPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            allProducts = Core.Context.Product.ToList();

            typeCB.ItemsSource = Core.Context.ProductType.ToList();
            manufacturerCB.ItemsSource = Core.Context.Manufacturer.ToList();

            UpdateUI();
        }

        private void UpdateUI()
        {
            var products = allProducts.ToList();

            if (!string.IsNullOrEmpty(searchTB.Text))
                products = products.Where(p => p.Name.ToLower().Contains(searchTB.Text.ToLower())).ToList();

            if (typeCB.SelectedItem != null)
            {
                var type = typeCB.SelectedItem as ProductType;
                products = products.Where(p => p.ProductTypeID == type.ID).ToList();
            }

            if (manufacturerCB.SelectedItem != null)
            {
                var man = manufacturerCB.SelectedItem as Manufacturer;
                products = products.Where(p => p.ManufacturerID == man.ID).ToList();
            }

            if (sortCB.SelectedIndex == 1)
                products = products.OrderBy(p => p.Price).ToList();

            if (sortCB.SelectedIndex == 2)
                products = products.OrderByDescending(p => p.Price).ToList();

            var displayProducts = products.Select(p => new
            {
                p.ID,
                p.Name,
                p.Price,
                p.ImagePath,
                CardColor = p.IsFreeze ? "#A3A3A3" : ((p.Discount.HasValue && p.Discount > 15) ? "#B4776E" : "#617891")
            }).ToList();

            productsList.ItemsSource = displayProducts;
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
        }

        private void TypeCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void ManufacturerCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void SortCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void addToBasketBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var product = button.DataContext as Product;
            if (product == null) return;

            if (!userBasket.AddProduct(product))
            {
                MessageBox.Show("Этот товар временно недоступен (заморожен).");
                return;
            }

            MessageBox.Show("Товар добавлен в корзину!");
        }

        private void toProductDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var product = button.DataContext as Product;
            if (product == null) return;

            selectedProduct = product;

            var detailsWindow = new chosenProductWindow(selectedProduct, userBasket);
            NavigationService.Navigate(detailsWindow);
        }

        private void toBasketBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new basketPage(userBasket));
        }

        private void backToClientMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
