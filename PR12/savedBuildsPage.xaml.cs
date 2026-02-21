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
    /// Логика взаимодействия для savedBuildsPage.xaml
    /// </summary>
    public partial class savedBuildsPage : Page
    {
        public savedBuildsPage()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void LoadHistory()
        {
            var builds = Core.Context.assembly_.Include("partassembly_.basepart_").ToList();
            buildHistoryList.ItemsSource = builds;
        }

        private void toMainBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
