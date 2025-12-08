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
    /// Логика взаимодействия для colorDopPage.xaml
    /// </summary>

    public partial class colorDopPage : Page
    {
        public int StepNumber => 2;

        private CarConfiguration config;
        private MainWindow wnd;

        private readonly Dictionary<string, double> carPrices = new Dictionary<string, double>()
        {
            { "Toyota", 2500000.0 },
            { "Mazda", 2300000.0 },
            { "BMW",   3500000.0 }
        };

        private readonly Dictionary<string, double> colorPrices = new Dictionary<string, double>()
        {
            { "Белый",        0.0 },
            { "Чёрный",       5000.0 },
            { "Серебристый",  7000.0 },
            { "Тёмно-синий",  9000.0 },
            { "Розовый",     15000.0 }
        };

        private Dictionary<CheckBox, (string, double)> optionMap;

        public colorDopPage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;

            wnd.SetStep(StepNumber);

            optionMap = new Dictionary<CheckBox, (string, double)>()
            {
                { tonChckBx, ("Тонировка", 8500.0) },
                { signChckBx, ("Сигнализация", 15000.0) },
                { anticorChckBx, ("Антикоррозийная обработка", 12000.0) },
                { wheelsChckBx, ("Зимние колёса", 13000.0) }
            };

            RestoreState();

            UpdateNextButtonState();
        }

        private void RestoreState()
        {
            switch (config.SelectedCar)
            {
                case "Toyota": toyotaRB.IsChecked = true; break;
                case "Mazda": mazdaRB.IsChecked = true; break;
                case "BMW": bmwRB.IsChecked = true; break;
            }

            switch (config.SelectedColor)
            {
                case "Белый": whiteRB.IsChecked = true; break;
                case "Чёрный": blackRB.IsChecked = true; break;
                case "Серебристый": serebroRB.IsChecked = true; break;
                case "Тёмно-синий": blueRB.IsChecked = true; break;
                case "Розовый": pinkRB.IsChecked = true; break;
            }

            foreach (var kv in optionMap)
            {
                kv.Key.Checked -= OptionChanged;
                kv.Key.Unchecked -= OptionChanged;

                kv.Key.IsChecked = config.SelectedOptions.Contains(kv.Value.Item1);

                kv.Key.Checked += OptionChanged;
                kv.Key.Unchecked += OptionChanged;
            }
        }

        private void car_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
            {

                string content = rb.Content.ToString();
                string key = "";

                switch (content)
                {
                    case "Toyota":
                    case string s when s.StartsWith("Toyota"):
                        key = "Toyota";
                        break;

                    case "Mazda":
                    case string s when s.StartsWith("Mazda"):
                        key = "Mazda";
                        break;

                    case "BMW":
                    case string s when s.StartsWith("BMW"):
                        key = "BMW";
                        break;

                    default:
                        key = content;
                        break;
                }

                config.SelectedCar = key;
                config.SelectedCarPrice = carPrices.ContainsKey(key) ? carPrices[key] : 0.0;
            }

            UpdateNextButtonState();
        }

        private void color_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                string color = rb.Content.ToString();

                switch (color)
                {
                    case "Белый":
                    case "Чёрный":
                    case "Серебристый":
                    case "Тёмно-синий":
                    case "Розовый":
                        config.SelectedColor = color;
                        config.SelectedColorPrice = colorPrices[color];
                        break;

                    default:
                        config.SelectedColor = color;
                        config.SelectedColorPrice = 0.0;
                        break;
                }
            }

            UpdateNextButtonState();
        }

        private void OptionChanged(object sender, RoutedEventArgs e)
        {
            double total = 0.0;
            List<string> selectedOptions = new List<string>();

            foreach (var kv in optionMap)
            {
                if (kv.Key.IsChecked == true)
                {
                    selectedOptions.Add(kv.Value.Item1);
                    total += kv.Value.Item2;
                }
            }

            config.SelectedOptions = selectedOptions;
            config.SelectedOptionsPrice = total;
        }

        private void UpdateNextButtonState()
        {
            bool ok = !string.IsNullOrEmpty(config.SelectedCar) &&
                      !string.IsNullOrEmpty(config.SelectedColor);

            nextButton.IsEnabled = ok;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new enginePage(config, wnd));
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (!nextButton.IsEnabled)
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль и цвет.");
                return;
            }

            OptionChanged(null, null);

            NavigationService.Navigate(new totalCostPage(config, wnd));
        }
    }
}


