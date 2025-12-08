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
    /// Логика взаимодействия для orderPage.xaml
    /// </summary>
    public partial class orderPage : Page
    {
        public int StepNumber => 5;

        private CarConfiguration config;
        private MainWindow wnd;
        private bool isDirty = false;

        public orderPage(CarConfiguration cfg, MainWindow w)
        {
            InitializeComponent();
            config = cfg;
            wnd = w;

            wnd.SetStep(StepNumber);

            userName.Text = config.UserName ?? "";
            userPhone.Text = config.UserPhone ?? "";
            userEmail.Text = config.UserEmail ?? "";

            userName.TextChanged += InputChanged;
            userPhone.TextChanged += InputChanged;
            userEmail.TextChanged += InputChanged;

            userPhone.PreviewTextInput += userPhone_PreviewTextInput;

            finishButton.IsEnabled = IsInputValid();
        }

        private void userPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            isDirty = true;
            finishButton.IsEnabled = IsInputValid();
        }

        private bool IsInputValid()
        {
            bool nameOk = !string.IsNullOrWhiteSpace(userName.Text) && userName.Text.Length >= 2 && !userName.Text.Any(char.IsDigit);
            bool phoneOk = !string.IsNullOrWhiteSpace(userPhone.Text) && userPhone.Text.Length >= 5 && userPhone.Text.All(char.IsDigit);
            bool emailOk = !string.IsNullOrWhiteSpace(userEmail.Text) && userEmail.Text.Length >= 5 && userEmail.Text.Contains("@") && userEmail.Text.Contains(".");

            return nameOk && phoneOk && emailOk;
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            if (!IsInputValid())
            {
                MessageBox.Show("Пожалуйста, корректно заполните все поля.", "Ошибка");
                return;
            }

            config.UserName = userName.Text.Trim();
            config.UserPhone = userPhone.Text.Trim();
            config.UserEmail = userEmail.Text.Trim();

            var result = MessageBox.Show(
                $"Заказ оформлен!\n\n" +
                $"Имя: {config.UserName}\n" +
                $"Телефон: {config.UserPhone}\n" +
                $"Почта: {config.UserEmail}\n\n" +
                $"Итоговая сумма: {config.TotalPrice:0} ₽\n" +
                $"Первоначальный взнос: {config.DownPaymentAmount:0} ₽\n" +
                $"Сумма кредита: {config.CreditAmount:0} ₽\n" +
                $"Ежемесячный платёж: {config.MonthlyPayment:0} ₽",
                "Успех", MessageBoxButton.OK);
            //if(result == MessageBoxResult.OK)
            //{
            //    Application.Current.Shutdown();
            //}
            //else
            //{
            //    return;
            //}
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty && !IsInputValid())
            {
                var r = MessageBox.Show("Данные заполнены не полностью или некорректны. Покинуть страницу и потерять введённые данные?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (r != MessageBoxResult.Yes) return;
            }

            NavigationService.Navigate(new creditPage(config, wnd));
        }
    }
}

