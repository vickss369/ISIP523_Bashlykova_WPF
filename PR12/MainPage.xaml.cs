using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private CarConfiguration config;
        private MainWindow wnd;

        public MainPage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;


            if (wnd.CurrentStep == 0)
            {
                bckToOrderButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                bckToOrderButton.Visibility = Visibility.Visible;
            }
        }

        private void engineButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new enginePage(config, wnd));
        }

        private void bckToOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            switch (wnd.CurrentStep)
            {
                case 1: NavigationService.Navigate(new enginePage(config, wnd)); break;
                case 2: NavigationService.Navigate(new colorDopPage(config, wnd)); break;
                case 3: NavigationService.Navigate(new totalCostPage(config, wnd)); break;
                case 4: NavigationService.Navigate(new creditPage(config, wnd)); break;
                case 5: NavigationService.Navigate(new orderPage(config, wnd)); break;
                default: NavigationService.Navigate(new enginePage(config, wnd)); break;
            }
        }
    }
}