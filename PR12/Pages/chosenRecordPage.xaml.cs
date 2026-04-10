using PR12.Classes;
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

namespace PR12.Pages
{
    /// <summary>
    /// Логика взаимодействия для chosenRecordPage.xaml
    /// </summary>
    public partial class chosenRecordPage : Page
    {
        private int timetableID;
        private int serviceID;

        private Timetable currentSlot;
        private Service currentService;
        private int selectedPaymentTypeID = 0;

        public chosenRecordPage(int timetableID, int serviceID)
        {
            InitializeComponent();
            recordBtn.IsEnabled = false;
            if (timetableID == 0 || serviceID == 0)
            {
                MessageBox.Show("Некорректные данные");
                return;
            }

            this.timetableID = timetableID;
            this.serviceID = serviceID;

            LoadRecordInfo();
        }

        private void LoadRecordInfo()
        {
            currentSlot = Core.Context.Timetable.FirstOrDefault(t => t.ID == timetableID);
            currentService = Core.Context.Service.FirstOrDefault(s => s.ID == serviceID);

            if (currentSlot == null || currentService == null)
            {
                MessageBox.Show("Ошибка");

                if (this.NavigationService != null)
                    this.NavigationService.GoBack();

                return;
            }

            var master = Core.Context.User.FirstOrDefault(u => u.ID == currentSlot.MasterID);
            var serviceType = Core.Context.ServiceType.FirstOrDefault(st => st.ID == currentService.ServiceTypeID);

            DataContext = new
            {
                Name = currentService.Name,
                ServiceTypeName = serviceType?.Name,
                MasterName = master?.FullName,
                Price = currentService.Price,

                DateText = $"Дата: {currentSlot.StartDateTime:dd.MM.yyyy}",
                StartTimeText = currentSlot.StartDateTime.ToString("HH:mm"),
                EndTimeText = currentSlot.EndDateTime.ToString("HH:mm"),

                ImagePath = currentService.ImagePath
            };
        }

        private void paymentTypeRB_Checked(object sender, RoutedEventArgs e)
        {
            if (cardRB.IsChecked == true) selectedPaymentTypeID = 1;
            if (cashRB.IsChecked == true) selectedPaymentTypeID = 2;
            if (sbpRB.IsChecked == true) selectedPaymentTypeID = 3;

            recordBtn.IsEnabled = true;
        }

        private void recordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (User.currentUser == null)
            {
                MessageBox.Show("Войдите в аккаунт");
                return;
            }

            if (selectedPaymentTypeID == 0)
            {
                MessageBox.Show("Выберите оплату");
                return;
            }

            if (currentSlot.ISBooked)
            {
                MessageBox.Show("Время занято");
                return;
            }

            Record record = new Record()
            {
                ClientID = User.currentUser.ID,
                TimetableID = currentSlot.ID,
                ServiceID = currentService.ID,
                RecordStatusID = 1,
                Comment = commentTB.Text,
                PaymentTypeID = selectedPaymentTypeID
            };

            Core.Context.Record.Add(record);
            currentSlot.ISBooked = true;

            Core.Context.SaveChanges();

            MessageBox.Show("Готово!");
            NavigationService.GoBack();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
