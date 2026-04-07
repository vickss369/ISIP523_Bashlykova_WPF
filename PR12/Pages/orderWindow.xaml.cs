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

            deliveryDatePicker.SelectedDate = DateTime.Now;
            deliveryDatePicker.DisplayDateStart = DateTime.Now;
            deliveryDatePicker.DisplayDateEnd = DateTime.Now.AddDays(7);
        }

        private void FillFields()
        {
            if (User.currentUser != null) userNameTBl.Text = User.currentUser.FullName;

            double total = Basket.currentBasket.ProductsInBasket.Sum(p => Basket.GetPriceWithDiscount(p));
            totalSumTBl.Text = total + "₽";

            List<string> productsInfo = new List<string>();
            var groupedProducts = Basket.currentBasket.ProductsInBasket.GroupBy(p => p.ID);
            foreach (var g in groupedProducts)
            {
                var item = g.First();
                double price = Basket.GetPriceWithDiscount(item);

                if (item.Discount != null && item.Discount > 0) 
                    productsInfo.Add($"{item.Name} x{g.Count()} ({price}₽, скидка {item.Discount}%)");
                else
                    productsInfo.Add($"{item.Name} x{g.Count()} ({price}₽)");
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

            if (deliveryDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату получения!");
                return;
            }

            Order newOrder = new Order
            {
                UserID = User.currentUser.ID,
                OrderDate = DateTime.Now,
                DeliveryDate = deliveryDatePicker.SelectedDate.Value,
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
                    PriceAtBuyMoment = Basket.GetPriceWithDiscount(product)
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