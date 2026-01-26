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

namespace PR12
{
    /// <summary>
    /// Логика взаимодействия для accountPage.xaml
    /// </summary>
    public partial class accountPage : Page
    {
        public accountPage()
        {
            InitializeComponent();
        }

        private void goTofilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new filmsPage());
        }
    }
}
