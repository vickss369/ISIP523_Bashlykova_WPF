using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12
{
    public class CarConfiguration
    {

        // Шаг 1: двигатель
        public string SelectedEngineType { get; set; } = "";
        public string SelectedEngineModel { get; set; } = "";
        public double SelectedEnginePrice { get; set; } = 0.0;

        // Шаг 2: автомобиль, цвет, опции
        public string SelectedCar { get; set; } = "";
        public double SelectedCarPrice { get; set; } = 0.0;

        public string SelectedColor { get; set; } = "";
        public double SelectedColorPrice { get; set; } = 0.0;

        public List<string> SelectedOptions { get; set; } = new List<string>();
        public double SelectedOptionsPrice { get; set; } = 0.0;

        //  Шаг 3: Итог
        public double TotalPrice => SelectedCarPrice + SelectedEnginePrice + SelectedColorPrice + SelectedOptionsPrice;

        //  Шаг 4: Кредит 
        public double DownPaymentPercent { get; set; } = 10.0; //%
        public int CreditTermMonths { get; set; } = 36;
        public double AnnualInterestRate { get; set; } = 12.0; //% годовых по умолчанию

        public double DownPaymentAmount => Math.Round(TotalPrice * (DownPaymentPercent / 100.0));
        public double CreditAmount => Math.Round(TotalPrice - DownPaymentAmount);

        public double MonthlyPayment { get; private set; } = 0.0;

        // Шаг 5: Данные пользователя
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }

        public void CalculateMonthlyPayment()
        {
            int n = CreditTermMonths;
            double r = AnnualInterestRate;
            double S = CreditAmount;
            if (n <= 0 || S <= 0)
            {
                MonthlyPayment = 0;
                return;
            }

            double i = r / 100.0 / 12.0;
            if (i == 0)
            {
                MonthlyPayment = S / n;
                return;
            }

            double pow = Math.Pow(1 + i, n);
            double A = S * (i * pow) / (pow - 1);
            MonthlyPayment = Math.Round(A);
        }
    }
}
