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
    /// Логика взаимодействия для orderWindow.xaml
    /// </summary>
    public partial class orderWindow : Window
    {
        private int selectedPaymentTypeID = 0;

        public orderWindow()
        {
            InitializeComponent();
            FillFields();
        }

        private void FillFields()
        {
            if (User.currentUser != null) userNameTBl.Text = User.currentUser.FullName;

            totalSumTBl.Text = Basket.currentBasket.TotalPrice + "₽";

            List<string> productsInfo = new List<string>();
            foreach (var item in Basket.currentBasket.ProductsInBasket)
            {
                productsInfo.Add($"{item.Name} ({item.Price}₽)");
            }

            orderedProductsTBl.Text = string.Join("\n", productsInfo);
        }

        private void paymentTypeRB_Checked(object sender, RoutedEventArgs e)
        {
            if (cardRB.IsChecked == true) selectedPaymentTypeID = 1;

            if (cashRB.IsChecked == true) selectedPaymentTypeID = 2;

            if (sbpRB.IsChecked == true) selectedPaymentTypeID = 3;

            finishBtn.IsEnabled = true;
        }

        private void finishOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPaymentTypeID == 0)
            {
                MessageBox.Show("Выберите тип оплаты!");
                return;
            }

            Order newOrder = new Order
            {
                UserID = User.currentUser.ID,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(2),
                PaymentTypeID = selectedPaymentTypeID,
                OrderStatus = "Оформлен",
                IsTaken = false
            };

            Core.Context.Order.Add(newOrder);
            Core.Context.SaveChanges();

            foreach (var product in Basket.currentBasket.ProductsInBasket)
            {
                OrderProduct op = new OrderProduct
                {
                    OrderID = newOrder.ID,
                    ProductID = product.ID,
                    Quantity = 1,
                    PriceAtBuyMoment = product.Price
                };
                Core.Context.OrderProduct.Add(op);
            }

            Core.Context.SaveChanges();

            MessageBox.Show("Заказ успешно оформлен!");
            Basket.currentBasket.ProductsInBasket.Clear();
            
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}