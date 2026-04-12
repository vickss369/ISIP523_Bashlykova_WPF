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
    /// Логика взаимодействия для managerPage.xaml
    /// </summary>
    public partial class managerPage : Page
    {
        private string currentMode = "";

        public managerPage()
        {
            InitializeComponent();
            managerNameTBl.Text = User.currentUser?.FullName;
            LoadServices();
        }

        private void servicesBtn_Click(object sender, RoutedEventArgs e) => LoadServices();
        private void productsBtn_Click(object sender, RoutedEventArgs e) => LoadProducts();
        private void recordsBtn_Click(object sender, RoutedEventArgs e) => LoadRecords();
        private void ordersBtn_Click(object sender, RoutedEventArgs e) => LoadOrders();
        private void manufacturersBtn_Click(object sender, RoutedEventArgs e) => LoadManufacturers();
        private void serviceTypesBtn_Click(object sender, RoutedEventArgs e) => LoadServiceTypes();
        private void productTypesBtn_Click(object sender, RoutedEventArgs e) => LoadProductTypes();


        private void LoadServices()
        {
            currentMode = "services";
            managerList.ItemsSource = Core.Context.Service.ToList().Select(s => new
            {
                ID = s.ID,
                Name = s.Name,
                TypeInfo = s.ServiceType.Name,
                PriceText = "Цена: " + s.Price + "₽",
                ClientText = "",
                DateText = "",
                ExtraInfo = "",
                ImagePath = s.ImagePath,
                ImageVisibility = Visibility.Visible,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Visible,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Collapsed,
                ActionText = "Изменить",
                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D4A5BB")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6E0"))
            }).ToList();
        }

        private void LoadProducts()
        {
            currentMode = "products";
            managerList.ItemsSource = Core.Context.Product.ToList().Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                TypeInfo = p.ProductType.Name,
                PriceText = "Цена: " + p.Price + "₽",
                ClientText = "",
                DateText = "",
                ExtraInfo = p.Manufacturer.Name,
                ImagePath = p.ImagePath,
                ImageVisibility = Visibility.Visible,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Visible,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Collapsed,
                ActionText = "Изменить",
                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4A261")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5B4"))
            }).ToList();
        }

        private void LoadRecords()
        {
            currentMode = "records";
            managerList.ItemsSource = Core.Context.Record.ToList().Select(r => new
            {
                ID = r.ID,
                Name = r.Service.Name,
                TypeInfo = r.Service.ServiceType.Name,
                PriceText = "Цена: " + r.Service.Price + "₽",
                ClientText = "Клиент: " + r.User.FullName,
                DateText = r.Timetable.StartDateTime.ToString("dd.MM.yyyy HH:mm"),
                ExtraInfo = "",
                ImagePath = r.Service.ImagePath,
                ImageVisibility = Visibility.Visible,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Visible,
                ClientVisibility = Visibility.Visible,
                DateVisibility = Visibility.Visible,
                ActionText = "Управление",
                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c47f78")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d69b95"))
            }).ToList();
        }

        private void LoadOrders()
        {
            currentMode = "orders";
            managerList.ItemsSource = Core.Context.Order.ToList().Select(o => new
            {
                ID = o.ID,
                Name = "Заказ №" + o.ID,
                TypeInfo = "",
                PriceText = "Сумма: " + o.OrderProduct.Sum(x => x.PriceAtBuyMoment * x.Quantity) + "₽",
                ClientText = "",
                DateText = o.OrderDate.ToString("dd.MM.yyyy"),
                ExtraInfo = "Статус: " + o.OrderStatus,
                ImagePath = "",
                ImageVisibility = Visibility.Collapsed,
                TypeVisibility = Visibility.Collapsed,
                PriceVisibility = Visibility.Visible,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Visible,
                ActionText = o.IsTaken ? "Закрыт" : "Закрыть",
                ButtonColor = o.IsTaken
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"))
                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FB069")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BFD397"))
            }).ToList();
        }

        private void LoadManufacturers()
        {
            currentMode = "manufacturers";
            managerList.ItemsSource = Core.Context.Manufacturer.ToList().Select(m => new
            {
                ID = m.ID,
                Name = m.Name,
                TypeInfo = "Производитель",
                PriceText = "",
                ClientText = "",
                DateText = "",
                ExtraInfo = "",
                ImagePath = "",
                ImageVisibility = Visibility.Collapsed,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Collapsed,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Collapsed,
                ActionText = "Изменить",
                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9a86c7")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B4A3D7"))
            }).ToList();
        }

        private void LoadServiceTypes()
        {
            currentMode = "serviceTypes";
            managerList.ItemsSource = Core.Context.ServiceType.ToList().Select(t => new
            {
                ID = t.ID,
                Name = t.Name,
                TypeInfo = "Тип услуги",
                PriceText = "Цена: от 950₽",
                ClientText = "",
                DateText = "",
                ExtraInfo = "",
                ImagePath = "",
                ImageVisibility = Visibility.Collapsed,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Visible,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Collapsed,
                ActionText = "Изменить",
                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4f8a97")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60A1AF"))
            }).ToList();
        }

        private void LoadProductTypes()
        {
            currentMode = "productTypes";

            managerList.ItemsSource = Core.Context.ProductType.ToList().Select(t => new
            {
                ID = t.ID,
                Name = t.Name,

                TypeInfo = "Тип товара",
                PriceText = "",
                ClientText = "",
                DateText = "",
                ExtraInfo = "",
                ImagePath = "",

                ImageVisibility = Visibility.Collapsed,
                TypeVisibility = Visibility.Visible,
                PriceVisibility = Visibility.Collapsed,
                ClientVisibility = Visibility.Collapsed,
                DateVisibility = Visibility.Collapsed,

                ActionText = "Изменить",

                ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c89a6b")),
                CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dab186"))
            }).ToList();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            dynamic item = btn.DataContext;
            int id = item.ID;

            var win = new managerEditWindow(currentMode, id);
            win.ShowDialog();

            if (win.IsChanged) ReloadCurrentList();
        }

        private void ReloadCurrentList()
        {
            if (currentMode == "services") LoadServices();

            else if (currentMode == "products") LoadProducts();

            else if (currentMode == "records") LoadRecords();

            else if (currentMode == "orders") LoadOrders();

            else if (currentMode == "manufacturers") LoadManufacturers();

            else if (currentMode == "serviceTypes") LoadServiceTypes();

            else if (currentMode == "productTypes") LoadProductTypes();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            User.currentUser = null;
            NavigationService.Navigate(new enterPage());
        }
    }
}
