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
    /// Логика взаимодействия для creditPage.xaml
    /// </summary>
    public partial class creditPage : Page
    {
        public int StepNumber => 4;

        private CarConfiguration config;
        private MainWindow wnd;

        public creditPage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;

            wnd.SetStep(StepNumber);

            depositSlider.Value = config.DownPaymentPercent;
            termSlider.Value = config.CreditTermMonths <= 0 ? 12 : config.CreditTermMonths;

            UpdateUI();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (config == null || depositSlider == null || termSlider == null)
                return;

            config.DownPaymentPercent = depositSlider.Value;
            config.CreditTermMonths = (int)Math.Round(termSlider.Value);

            config.CalculateMonthlyPayment();
            UpdateUI();
        }

        private void UpdateUI()
        {
            depositPercentText.Text = $"{config.DownPaymentPercent:0} %";
            termText.Text = $"{config.CreditTermMonths} мес.";

            carPriceText.Text = $"{config.TotalPrice:0}";
            depositAmountText.Text = $"{config.DownPaymentAmount:0}";
            loanAmountText.Text = $"{config.CreditAmount:0}";
            termAmountText.Text = $"{config.CreditTermMonths}";
            monthlyPaymentText.Text = $"{config.MonthlyPayment:0}";
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new totalCostPage(config, wnd));
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new orderPage(config, wnd));
        }
    }
}
