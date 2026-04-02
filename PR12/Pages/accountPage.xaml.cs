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
using PR12.Classes;

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для accountPage.xaml
    /// </summary>
    public partial class accountPage : Page
    {
        private User currentUser;

        public accountPage()
        {
            InitializeComponent();
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            currentUser = User.currentUser;
            if (currentUser == null)
            {
                return;
            }

            UserNameTB.Text = currentUser.FullName;
            phoneNumberTB.Text = currentUser.PhoneNumber;

            var records = Core.Context.Record.Where(r => r.ClientID == currentUser.ID).ToList();
            recordsList.ItemsSource = records;

            var products = Core.Context.Product.ToList();
            productsList.ItemsSource = products;
        }

        private void backToClientMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new clientPage());
        }
    }
}
