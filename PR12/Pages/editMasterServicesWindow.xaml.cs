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
    /// Логика взаимодействия для editMasterServicesWindow.xaml
    /// </summary>
    public partial class editMasterServicesWindow : Window
    {
        public editMasterServicesWindow()
        {
            InitializeComponent();
            LoadServices();
        }

        private void LoadServices()
        {
            if (User.currentUser == null) return;

            var available = MasterServicesManager.GetAvailableServices();
            var myServices = MasterServicesManager.GetMyServices();

            var items = new List<object>();

            foreach (var s in available)
            {
                bool selected = myServices.Any(x => x.ID == s.ID);

                items.Add(new
                {
                    ID = s.ID,
                    Name = s.Name,
                    Price = s.Price,
                    ImagePath = s.ImagePath,
                    ButtonText = selected ? "Убрать" : "Добавить",
                    ButtonColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selected ? "#C05959" : "#8ABC7B")),
                    CardColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFBCA4"))
                });
            }

            servicesList.ItemsSource = items;
        }

        private void ToggleService_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            dynamic item = btn.DataContext;

            bool isAdding = item.ButtonText == "Добавить";

            MasterServicesManager.ToggleService(item.ID, isAdding);

            LoadServices();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Список ваших услуг обновлён!");
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
