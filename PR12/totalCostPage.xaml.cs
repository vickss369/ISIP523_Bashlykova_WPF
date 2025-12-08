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
    /// Логика взаимодействия для totalCostPage.xaml
    /// </summary>
    public partial class totalCostPage : Page
    {
        public int StepNumber => 3;

        private CarConfiguration config;
        private MainWindow wnd;

        public totalCostPage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;
            wnd.SetStep(StepNumber);

            FillFields();
        }

        private void FillFields()
        {
            chosenAutoFull.Text = $"{config.SelectedCar} ({config.SelectedCarPrice} ₽)";

            chosenEngineFull.Text = $"{config.SelectedEngineModel} ({config.SelectedEnginePrice} ₽)";

            chosenColorFull.Text = $"{config.SelectedColor} ({config.SelectedColorPrice} ₽)";

            if (config.SelectedOptions.Count == 0)
            {
                chosenDopFull.Text = "— (0 ₽)";
            }
            else
            {
                string list = string.Join(", ", config.SelectedOptions);
                chosenDopFull.Text = $"{list} ({config.SelectedOptionsPrice} ₽)";
            }

            totalFull.Text = $"{config.TotalPrice} ₽";
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new colorDopPage(config, wnd));
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new creditPage(config, wnd));
        }
    }
}
