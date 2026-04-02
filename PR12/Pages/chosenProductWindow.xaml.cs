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
using System.Windows.Shapes;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для chosenProductWindow.xaml
    /// </summary>
    public partial class chosenProductWindow : Window
    {
        private Product selectedProduct;
        private Basket userBasket;

        public chosenProductWindow(Product product, Basket basket)
        {
            InitializeComponent();
            selectedProduct = product;
            userBasket = basket;
        }
        }
}
