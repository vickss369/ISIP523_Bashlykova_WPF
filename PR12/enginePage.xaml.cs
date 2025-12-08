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
    /// Логика взаимодействия для enginePage.xaml
    /// </summary>
    public partial class enginePage : Page
    {
        public int StepNumber => 1;

        private CarConfiguration config;
        private MainWindow wnd;

        public enginePage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;

            wnd.SetStep(StepNumber);

            if (!string.IsNullOrEmpty(config.SelectedEngineType))
            {
                if (config.SelectedEngineType == "Бензиновый") benzRB.IsChecked = true;
                else if (config.SelectedEngineType == "Дизельный") dizelRB.IsChecked = true;
                else if (config.SelectedEngineType == "Электрический") electroRB.IsChecked = true;
            }

            if (!string.IsNullOrEmpty(config.SelectedEngineModel))
            {
                if (config.SelectedEngineModel == superRB.Content.ToString()) superRB.IsChecked = true;
                else if (config.SelectedEngineModel == allMetalRB.Content.ToString()) allMetalRB.IsChecked = true;
                else if (config.SelectedEngineModel == VAZRB.Content.ToString()) VAZRB.IsChecked = true;
            }

            UpdateNextButtonState();
        }

        private void engineType_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null) return;
            config.SelectedEngineType = rb.Content.ToString();
            UpdateNextButtonState();
        }

        private void engineModel_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb == null) return;
            config.SelectedEngineModel = rb.Content.ToString();

            string model = config.SelectedEngineModel;

            switch (model)
            {
                case "Supercharged V8 American Engine":
                    config.SelectedEnginePrice = 14000.0;
                    break;

                case "Car Engine Model All-Metal":
                    config.SelectedEnginePrice = 30000.0;
                    break;

                case "Двигатель АвтоВАЗ 21213 V-1700":
                    config.SelectedEnginePrice = 130000.0;
                    break;

                default:
                    config.SelectedEnginePrice = 0.0;
                    break;
            }

            UpdateNextButtonState();
        }

        private void UpdateNextButtonState()
        {
            nextButton.IsEnabled = !string.IsNullOrEmpty(config.SelectedEngineType)
                                   && !string.IsNullOrEmpty(config.SelectedEngineModel);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(config, wnd));
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (!nextButton.IsEnabled)
            {
                MessageBox.Show("Выберите тип и модель двигателя.");
                return;
            }

            NavigationService.Navigate(new colorDopPage(config, wnd));
        }
    }
}
