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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CarConfiguration Config { get; private set; } = new CarConfiguration();

        public int CurrentStep { get; private set; } = 0;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage(Config, this));
        }

        public void SetStep(int step)
        {
            CurrentStep = step;
            StepBar.Visibility = Visibility.Visible;
            StepBar.Value = step;
        }

        private void globalBackButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage(Config, this));
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is MainPage)
            {
                globalBackButton.Visibility = Visibility.Collapsed;
                StepBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                globalBackButton.Visibility = Visibility.Visible;
                StepBar.Visibility = Visibility.Visible;
            }
        }
    }
}
